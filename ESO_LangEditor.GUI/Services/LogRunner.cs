using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.GUI.Services
{
    public class LogRunner
    {
        private readonly ILogger<LogRunner> _logger;

        public LogRunner(ILogger<LogRunner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogDebug(20, "Doing hard work! {Action}", name);
        }

    }
}
