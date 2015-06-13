using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;
using EnvCrypt.Core.Verb.LoadDat;

namespace EnvCrypt.Core.Verb.DecryptEntry.Generic
{
    /// <summary>
    /// To decrypt many entries with one key.  PlainText will be retrieved (never requires a a key.
    /// </summary>
    public class DecryptGenericWorkflow
    {
        private readonly IDatLoader _datLoader;
        private readonly IDecryptPlainTextEntryWorkflowBuilder _plaintextWorkFlowBuilder;
        private readonly IDecryptRsaEntryWorkflowBuilder _rsaWorkFlowBuilder;
        private readonly IDecryptAesEntryWorkflowBuilder _aesWorkFlowBuilder;


        public DecryptGenericWorkflow(IDatLoader datLoader, IDecryptPlainTextEntryWorkflowBuilder plaintextWorkFlowBuilder, IDecryptRsaEntryWorkflowBuilder rsaWorkFlowBuilder, IDecryptAesEntryWorkflowBuilder aesWorkFlowBuilder)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(plaintextWorkFlowBuilder != null, "plaintextWorkFlowBuilder");
            Contract.Requires<ArgumentNullException>(rsaWorkFlowBuilder != null, "rsaWorkFlowBuilder");
            Contract.Requires<ArgumentNullException>(aesWorkFlowBuilder != null, "aesWorkFlowBuilder");
            //
            _datLoader = datLoader;
            _plaintextWorkFlowBuilder = plaintextWorkFlowBuilder;
            _rsaWorkFlowBuilder = rsaWorkFlowBuilder;
            _aesWorkFlowBuilder = aesWorkFlowBuilder;
        }


        public IList<EntriesDecrypterResult> Run(DecryptGenericWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<EnvCryptException>(options.CategoryEntryPair.Any(),
                "at least one entry has to be requested");
            //
            var dat = _datLoader.Load(options.DatFilePath);

            // Get algo used for all requested entries
            var detailsOfAlgoUsedTakenFromDat = new List<PairWithEncyptionAlgo>(options.CategoryEntryPair.Count);
            EnvCryptAlgoEnum? algorithmThatIsNotPlainText = null;
            for (uint entryI = 0; entryI < options.CategoryEntryPair.Count; entryI++)
            {
                var currentRequestedPair = options.CategoryEntryPair[(int) entryI];

                Entry foundEntry;
                if (!dat.SearchForEntry(currentRequestedPair.Category, currentRequestedPair.Entry, out foundEntry))
                {
                    if (options.ThrowExceptionIfEntryNotFound)
                    {
                        throw new EnvCryptException("cannot find entry  {0}  in category  {1}",
                            currentRequestedPair.Category, currentRequestedPair.Entry);
                    }
                    continue;
                }

                // Check that all requested entries use the same encryption algorithm because we are only loading one key
                if (algorithmThatIsNotPlainText.HasValue)
                {
                    if (foundEntry.EncryptionAlgorithm != algorithmThatIsNotPlainText.Value)
                    {
                        throw new EnvCryptException(
                            "all requested entries to be decrypted must have the same encryption algorithm.  Since if they are not, one key cannot possibly decrypt for more than one algorithm");
                    }
                }
                else
                {
                    if (foundEntry.EncryptionAlgorithm != EnvCryptAlgoEnum.PlainText)
                    {
                        algorithmThatIsNotPlainText = foundEntry.EncryptionAlgorithm;
                    }
                }

                detailsOfAlgoUsedTakenFromDat.Add(new PairWithEncyptionAlgo(currentRequestedPair,
                    foundEntry.EncryptionAlgorithm));
            }


            var ret = new List<EntriesDecrypterResult>();
            DecryptPlainText(detailsOfAlgoUsedTakenFromDat, ret);

            DecryptRsa(options, detailsOfAlgoUsedTakenFromDat, ret);

            DecryptAes(options, detailsOfAlgoUsedTakenFromDat, ret);


            return ret;
        }


        private void DecryptAes(DecryptGenericWorkflowOptions options, List<PairWithEncyptionAlgo> detailsOfAlgoUsedTakenFromDat, List<EntriesDecrypterResult> ret)
        {
            var aesRequests =
                detailsOfAlgoUsedTakenFromDat.Where(d => d.EncryptionAlgo == EnvCryptAlgoEnum.Aes).ToArray();
            if (aesRequests.Any())
            {
                // Case: AES
                var workflowOptions = new DecryptEntryWorkflowOptions()
                {
                    CategoryEntryPair = aesRequests.Select(r => r.Pair).ToList(),
                    DatFilePath = "null",
                    KeyFilePaths = new[] {options.KeyFilePath}
                };
                var result = _aesWorkFlowBuilder.WithDatLoader(_datLoader)
                    .Build()
                    .Run(workflowOptions);
                ret.AddRange(result);
            }
        }


        private void DecryptRsa(DecryptGenericWorkflowOptions options, List<PairWithEncyptionAlgo> detailsOfAlgoUsedTakenFromDat, List<EntriesDecrypterResult> ret)
        {
            var rsaRequests =
                detailsOfAlgoUsedTakenFromDat.Where(d => d.EncryptionAlgo == EnvCryptAlgoEnum.Rsa).ToArray();
            if (rsaRequests.Any())
            {
                // Case: RSA
                var workflowOptions = new DecryptEntryWorkflowOptions()
                {
                    CategoryEntryPair = rsaRequests.Select(r => r.Pair).ToList(),
                    DatFilePath = "null",
                    KeyFilePaths = new[] {options.KeyFilePath}
                };
                var result = _rsaWorkFlowBuilder.WithDatLoader(_datLoader)
                    .Build()
                    .Run(workflowOptions);
                ret.AddRange(result);
            }
        }


        private void DecryptPlainText(List<PairWithEncyptionAlgo> detailsOfAlgoUsedTakenFromDat, List<EntriesDecrypterResult> ret)
        {
            var plainTextRequests =
                detailsOfAlgoUsedTakenFromDat.Where(d => d.EncryptionAlgo == EnvCryptAlgoEnum.PlainText).ToArray();

            if (plainTextRequests.Any())
            {
                // Case: PlainText
                var workflowOptions = new DecryptPlainTextEntryWorkflowOptions()
                {
                    CategoryEntryPair = plainTextRequests.Select(r => r.Pair).ToList(),
                    DatFilePath = null
                };
                var result = _plaintextWorkFlowBuilder.WithDatLoader(_datLoader)
                    .Build()
                    .Run(workflowOptions);
                ret.AddRange(result);
            }
        }


        /// <summary>
        /// Temporary data structure
        /// </summary>
        private class PairWithEncyptionAlgo
        {
            public CategoryEntryPair Pair;
            public EnvCryptAlgoEnum EncryptionAlgo;

            public PairWithEncyptionAlgo(CategoryEntryPair pair, EnvCryptAlgoEnum encryptionAlgo)
            {
                Pair = pair;
                EncryptionAlgo = encryptionAlgo;
            }
        }
    }
}
