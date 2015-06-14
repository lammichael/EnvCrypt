namespace EnvCrypt.Console.GenerateKey
{
    class GenerateKeyCommandLineProcessor : VerbCommandLineProcessor<GenerateKeyVerbOptions>
    {
        protected override bool ReportErrors(GenerateKeyVerbOptions options)
        {
            var hasErrors = false;
            if (options.GetAlgorithm() == null)
            {
                System.Console.Error.WriteLine("Unrecognised algorithm: {0}", options.AlgorithmToUse);
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.KeyName))
            {
                System.Console.Error.WriteLine("Key name not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.OutputDirectory))
            {
                System.Console.Error.WriteLine("Output directory not defined.");
                hasErrors = true;
            }

            return hasErrors;
        }


        protected override void RunWorkflow(GenerateKeyVerbOptions options)
        {
            new GenerateKeyWorkflow().Run(options);
        }
    }
}
