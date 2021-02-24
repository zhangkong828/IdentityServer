$(function () {

    function checkPassWord(value) {
        var arr = [], array = [1, 2, 3, 4];
        if (value.length < 6) {//长度小于6
            return 0;
        }
        if (/\d/.test(value)) {//包含了数字
            arr.push(1);
        }
        if (/[a-z]/.test(value)) {//包含了小写的a到z
            arr.push(2);
        }
        if (/[A-Z]/.test(value)) {//包含了大写的A到Z
            arr.push(3);
        }
        if (/\W/.test(value)) {//非数字、字母、下划线
            arr.push(4);
        }
        for (var i = 0; i < array.length; i++) {
            if (arr.indexOf(array[i]) === -1) {
                return array[i];
            }
        }
    }

    $(".preview-password").click(function () {
        var input = $(this).parent().find("input");
        var type = input.attr("type");
        if (type === "password") {
            input.attr("type", "text");
            $(this).removeClass("glyphicon-eye-close").addClass("glyphicon-eye-open");
        } else {
            input.attr("type", "password");
            $(this).removeClass("glyphicon-eye-open").addClass("glyphicon-eye-close");
        }
    });

    $("#reg_email_btn").click(function () {
        var mailReg = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;

        var nickname = $("#reg_email_nickname").val();
        var username = $("#reg_email_username").val();
        var password = $("#reg_email_password").val();
        var password2 = $("#reg_email_password2").val();

        if (!nickname || nickname.length === 0 || nickname.trim() === "") {
            alert("昵称不能为空");
            return false;
        }

        if (!username || username.length === 0 || username.trim() === "") {
            alert("请填写邮箱");
            return false;
        }

        if (!mailReg.test(username)) {
            alert("邮箱格式不正确");
            return false;
        }

        if (!password || password.length === 0 || password.trim() === "") {
            alert("请填写密码");
            return false;
        }

        var check = checkPassWord(password);
        if (check <= 3) {
            switch (check) {
                case 0:
                    alert("密码长度不能小于6");
                    return false;
                case 1:
                    alert("密码需要包含数字");
                    return false;
                case 2:
                    alert("密码需要包含小写字母");
                    return false;
                case 3:
                    alert("密码需要包含大写字母");
                    return false;
            }
        }

        if (!password2 || password2.length === 0 || password2.trim() === "") {
            alert("请填写重复密码");
            return false;
        }

        if (password !== password2) {
            alert("2次密码不一致");
            return false;
        }

        $.ajax({
            url: "/register",
            type: "post",
            data: { NickName: nickname, UserName: username, Password: password },
            success: function (response) {
                if (response.code === 0) {
                    window.location.href = "/";
                } else {
                    alert(response.msg);
                }
            }
        });
    });

    $("#login_btn").click(function () {
        var username = $("#username").val();
        var password = $("#password").val();
        var remember = $("#remember").prop("checked");
        var returnUrl = $("#returnUrl").val();

        if (!username || username.length === 0 || username.trim() === "") {
            alert("账号不能为空");
            return false;
        }

        if (!password || password.length === 0 || password.trim() === "") {
            alert("密码不能为空");
            return false;
        }

        $.ajax({
            url: "/login",
            type: "post",
            data: { UserName: username, Password: password, Remember: remember, ReturnUrl: returnUrl },
            success: function (response) {
                if (response.code === 0) {
                    window.location.href = response.returnUrl;
                } else {
                    alert(response.msg);
                }
            }
        });
    });
})