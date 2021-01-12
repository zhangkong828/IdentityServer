﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Utility
{
    public class ObjectId
    {
        //开始时间截 (2020-04-24 10:15:03) 
        const long Twepoch = 1587694503000L;
        //机器标识位数
        const int WorkerIdBits = 5;
        //数据中心标志位数
        const int DatacenterIdBits = 5;
        //序列号识位数
        const int SequenceBits = 12;
        //机器ID最大值:31
        const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        //数据中心标志ID最大值:31
        const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        //序列号ID最大值
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);
        //机器ID偏左移12位
        private const int WorkerIdShift = SequenceBits;
        //数据中心ID偏左移17位
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        //时间毫秒左移22位
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        //毫秒内序列(0~4095)
        private long _sequence = 0L;
        //上次生成ID的时间截
        private long _lastTimestamp = -1L;

        //工作机器ID(0~31)
        public long WorkerId { get; protected set; }
        //数据中心ID(0~31)
        public long DatacenterId { get; protected set; }
        public long Sequence
        {
            get { return _sequence; }
            internal set { _sequence = value; }
        }

        private static ObjectId _snowflakeId;
        private static readonly object SLock = new object();
        public static ObjectId Default()
        {
            lock (SLock)
            {
                if (_snowflakeId != null)
                {
                    return _snowflakeId;
                }

                var random = new Random();

                if (!int.TryParse(Environment.GetEnvironmentVariable("SchedulerZ_WORKERID", EnvironmentVariableTarget.Machine), out var workerId))
                {
                    workerId = random.Next((int)MaxWorkerId);
                }

                if (!int.TryParse(Environment.GetEnvironmentVariable("SchedulerZ_DATACENTERID", EnvironmentVariableTarget.Machine), out var datacenterId))
                {
                    datacenterId = random.Next((int)MaxDatacenterId);
                }

                return _snowflakeId = new ObjectId(workerId, datacenterId);
            }
        }

        public ObjectId(long workerId, long datacenterId, long sequence = 0L)
        {
            // 如果超出范围就抛出异常
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id 必须大于0，且不能大于MaxWorkerId： {0}", MaxWorkerId));
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(string.Format("region Id 必须大于0，且不能大于MaxWorkerId： {0}", MaxDatacenterId));
            }

            //先检验再赋值
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
        }


        readonly object _lock = new object();
        public virtual long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception(string.Format("时间戳必须大于上一次生成ID的时间戳.  拒绝为{0}毫秒生成id", _lastTimestamp - timestamp));
                }

                //如果上次生成时间和当前时间相同,在同一毫秒内
                if (_lastTimestamp == timestamp)
                {
                    //sequence自增，和sequenceMask相与一下，去掉高位
                    _sequence = (_sequence + 1) & SequenceMask;
                    //判断是否溢出,也就是每毫秒内超过1024，当为1024时，与sequenceMask相与，sequence就等于0
                    if (_sequence == 0)
                    {
                        //等待到下一毫秒
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    //如果和上次生成时间不同,重置sequence，就是下一毫秒开始，sequence计数重新从0开始累加,
                    //为了保证尾数随机性更大一些,最后一位可以设置一个随机数
                    _sequence = 0;//new Random().Next(10);
                }

                _lastTimestamp = timestamp;
                return ((timestamp - Twepoch) << TimestampLeftShift) | (DatacenterId << DatacenterIdShift) | (WorkerId << WorkerIdShift) | _sequence;
            }
        }

        public string NextString()
        {
            return ConvertTo62(NextId());
        }

        // 防止产生的时间比之前的时间还要小（由于NTP回拨等问题）,保持增量的趋势.
        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        // 获取当前的时间戳
        protected virtual long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private static char[] charSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        /// <summary>
        /// 将指定数字转换为指定长度的62进制
        /// </summary>
        /// <param name="value">要转换的数字</param>
        public static string ConvertTo62(long value)
        {
            string sixtyNum = string.Empty;
            long result = value;
            while (result > 0)
            {
                long val = result % 62;
                sixtyNum = charSet[val] + sixtyNum;
                result = result / 62;
            }
            return sixtyNum;
        }
    }
}
