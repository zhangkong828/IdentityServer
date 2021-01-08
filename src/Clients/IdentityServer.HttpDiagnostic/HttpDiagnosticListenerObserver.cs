using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.HttpDiagnostic
{
    public class HttpDiagnosticListenerObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(DiagnosticListener value)
        {
            value.Subscribe(new HttpDefaultObserver());
        }
    }
}
