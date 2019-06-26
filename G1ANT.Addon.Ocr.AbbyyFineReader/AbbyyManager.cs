/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr.AbbyyFineReader
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FREngine;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using G1ANT.Language;
using System.Diagnostics;

namespace G1ANT.Addon.Ocr.AbbyyFineReader
{
    public class AbbyyManager : IDisposable
    {
        public enum SettingsMode
        {
            None = 0,
            LineByLine = 1
        }

        public const string TextAccuracyProfile = "TextExtraction_Accuracy";
        public const string DocumentConversionAccuracyProfile = "DocumentConversion_Accuracy";
        private const string licens = "SWXT-1101-0006-2735-3250-7560";

        private static AbbyyManager instance = null;
        private static object syncRoot = new object();

        private Dictionary<int, FineReaderDocument> documents = new Dictionary<int, FineReaderDocument>();
        private int countID = 0;

        private IEngineLoader engineLoader;
        private IEngine engine;
        private PrepareImageMode prepareImageMode;

        private AbbyyManager()
        {
            engineLoader = new InprocLoader();
            engine = engineLoader.GetEngineObject(licens);

            prepareImageMode = engine.CreatePrepareImageMode();
            prepareImageMode.EnhanceLocalContrast = true;
        }

        public static AbbyyManager Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AbbyyManager();
                    }
                return instance;
            }
        }

        public IEngine Engine => engine;

        public FineReaderDocument CreateDocument(string path)
        {
            FineReaderDocument document = CreateDocument();
            document.Document.AddImageFile(path, PrepareMode: prepareImageMode);

            return document;
        }

        public FineReaderDocument CreateDocument(Stream stream)
        {
            stream.Position = 0;
            IReadStream abbyyStream = new StreamNet2AbbyyAdapter(stream);
            FineReaderDocument document = CreateDocument();
            document.Document.AddImageFileFromStream(abbyyStream, PrepareMode: prepareImageMode);

            return document;
        }

        private FineReaderDocument CreateDocument()
        {
            FineReaderDocument document = new FineReaderDocument(engine.CreateFRDocument(), ++countID);
            documents[countID] = document;

            return document;
        }

        public FineReaderDocument GetDocument(int id)
        {
            return documents[id];
        }

        public int CurentDocumentCount => countID;

        public void ProcessDocument(FineReaderDocument document, List<int> pageIndices, string language, int languageWeight, int dictionaryWeight, List<string> dictionary = null, SettingsMode mode = SettingsMode.None)
        {
            document.Document.Preprocess();
            if (pageIndices == null)
                document.Document.Process(GetProcessingParameters(language, mode, dictionary, languageWeight, dictionaryWeight));
            else
            {
                IntsCollection indices = engine.CreateIntsCollection();
                foreach (int i in pageIndices)
                    indices.Add(i);

                document.Document.ProcessPages(indices, GetProcessingParameters(language, mode, dictionary, languageWeight, dictionaryWeight));
            }
            document.InitializeCustomDocument();
        }

        private DocumentProcessingParams GetProcessingParameters(string language, SettingsMode mode, List<string> dictionary, int languageWeight, int dictionaryWeight)
        {
            engine.LoadPredefinedProfile(DocumentConversionAccuracyProfile);

            DocumentProcessingParams processingParams = engine.CreateDocumentProcessingParams();

            SetPageProcessingParameters(processingParams, language, dictionary, languageWeight, dictionaryWeight);

            SetSplittingParameters(processingParams, mode);

            SetDocumentSynthesisParameters(processingParams, mode);

            return processingParams;
        }

        private void SetDocumentSynthesisParameters(DocumentProcessingParams processingParams, SettingsMode mode)
        {
            SynthesisParamsForDocument documentSynthesisParams = processingParams.SynthesisParamsForDocument ?? engine.CreateSynthesisParamsForDocument();
            switch (mode)
            {
                case SettingsMode.None:
                    documentSynthesisParams.DetectFontFormatting = true;
                    documentSynthesisParams.DetectDocumentStructure = true;
                    break;
                case SettingsMode.LineByLine:
                    documentSynthesisParams.DetectFontFormatting = true;
                    documentSynthesisParams.DetectDocumentStructure = false;
                    break;
                default:
                    break;
            }

            SetDocumentStructureDetectionParameters(documentSynthesisParams, mode);
            SetFontFormatDetectionParameters(documentSynthesisParams, mode);
        }

        private void SetFontFormatDetectionParameters(SynthesisParamsForDocument documentSynthesisParams, SettingsMode mode)
        {
            FontFormattingDetectionParams fontFormattingDetectionParams = documentSynthesisParams.FontFormattingDetectionParams;
            switch (mode)
            {
                case SettingsMode.None:
                    fontFormattingDetectionParams.DetectBold = true;
                    fontFormattingDetectionParams.DetectFontFamily = true;
                    fontFormattingDetectionParams.DetectItalic = true;
                    fontFormattingDetectionParams.DetectFontSize = true;
                    fontFormattingDetectionParams.DetectUnderlineStrikeout = true;
                    break;
                case SettingsMode.LineByLine:
                    fontFormattingDetectionParams.DetectBold = true;
                    fontFormattingDetectionParams.DetectFontFamily = true;
                    fontFormattingDetectionParams.DetectItalic = true;
                    fontFormattingDetectionParams.DetectFontSize = true;
                    fontFormattingDetectionParams.DetectUnderlineStrikeout = true;
                    break;
                default:
                    break;
            }
        }

        private void SetDocumentStructureDetectionParameters(SynthesisParamsForDocument documentSynthesisParams, SettingsMode mode)
        {
            DocumentStructureDetectionParams documentStructureDetectionParams = documentSynthesisParams.DocumentStructureDetectionParams;
            switch (mode)
            {
                case SettingsMode.None:
                    break;
                case SettingsMode.LineByLine:
                    documentStructureDetectionParams.ClassifySeparators = true;
                    documentStructureDetectionParams.DetectCaptions = false;
                    documentStructureDetectionParams.DetectColumns = false;
                    documentStructureDetectionParams.DetectFootnotes = false;
                    documentStructureDetectionParams.DetectHeadlines = false;
                    documentStructureDetectionParams.DetectLists = false;
                    documentStructureDetectionParams.DetectOverflowingParagraphs = false;
                    documentStructureDetectionParams.DetectRunningTitles = false;
                    documentStructureDetectionParams.DetectTableOfContents = false;
                    break;
                default:
                    break;
            }
        }

        private void SetSplittingParameters(DocumentProcessingParams processingParams, SettingsMode mode)
        {
            PageSplittingParams pageSplittingParams = processingParams.SplittingParams ?? engine.CreatePageSplittingParams();
            processingParams.SplittingParams = pageSplittingParams;
        }

        private void SetPageProcessingParameters(DocumentProcessingParams processingParams, string language, List<string> dictionary, int languageWeight, int dictionaryWeight)
        {
            PageProcessingParams pageProcessingParams = processingParams.PageProcessingParams ?? engine.CreatePageProcessingParams();

            processingParams.PageProcessingParams = pageProcessingParams;

            SetPreProcessingParameters(pageProcessingParams);

            SetObjectExtractionParameters(pageProcessingParams);

            SetPageAnalyzeParameters(pageProcessingParams);

            SetPageSynthesisParameters(pageProcessingParams);

            SetRecognitionParameters(language, pageProcessingParams, dictionary, languageWeight, dictionaryWeight);
        }

        private void SetPreProcessingParameters(PageProcessingParams pageProcessingParams)
        {
            pageProcessingParams.PerformPreprocessing = true;
            pageProcessingParams.PagePreprocessingParams.CorrectOrientation = true;
            pageProcessingParams.PagePreprocessingParams.CorrectSkew = ThreeStatePropertyValueEnum.TSPV_Yes;
            pageProcessingParams.PagePreprocessingParams.CorrectSkewMode = (int)(CorrectSkewModeEnum.CSM_CorrectSkewByHorizontalText | CorrectSkewModeEnum.CSM_CorrectSkewByHorizontalLines);
        }

        private void SetPageSynthesisParameters(PageProcessingParams pageProcessingParams)
        {
            SynthesisParamsForPage pageSynthesisParams = pageProcessingParams.SynthesisParamsForPage ?? engine.CreateSynthesisParamsForPage();
            pageProcessingParams.SynthesisParamsForPage = pageSynthesisParams;

            FontFormattingDetectionParams fontFormattingDetectionParams = pageSynthesisParams.FontFormattingDetectionParams;
            fontFormattingDetectionParams.DetectBold = true;
            fontFormattingDetectionParams.DetectFontFamily = true;
            fontFormattingDetectionParams.DetectItalic = true;
            fontFormattingDetectionParams.DetectFontSize = true;
            fontFormattingDetectionParams.DetectUnderlineStrikeout = true;
        }

        private void SetRecognitionParameters(string language, PageProcessingParams pageProcessingParams, List<string> dictionary, int languageWeight, int dictionaryWeight)
        {
            RecognizerParams recognizingParams = pageProcessingParams.RecognizerParams ?? engine.CreateRecognizerParams();
            pageProcessingParams.RecognizerParams = recognizingParams;
            if (dictionary != null)
            {
                TextLanguage textLanguage = makeTextLanguage(language, dictionary, languageWeight, dictionaryWeight);
                recognizingParams.TextLanguage = textLanguage;
            }
            else
            {
                recognizingParams.WritingStyle = GetWritingSyle(language);
                recognizingParams.SetPredefinedTextLanguage(language);
            }
        }

        private TextLanguage makeTextLanguage(string language, List<string> dictionary, int languageWeight, int dictionaryWeight)
        {
            // Create new TextLanguage object
            LanguageDatabase languageDatabase = Engine.CreateLanguageDatabase();
            TextLanguage Language = languageDatabase.CreateTextLanguage();

            if (language != null)
            {
                // Copy all attributes from predefined language
                Language.CopyFrom(Engine.PredefinedLanguages.Find(language).TextLanguage);
                Language.InternalName = "SampleTextLanguage";
            }
            else
            {
                Language.CopyFrom(Engine.PredefinedLanguages.Find("English").TextLanguage);
            }

            // Bind new dictionary to first (and single) BaseLanguage object within TextLanguage
            //BaseLanguage baseLanguage = Language.BaseLanguages[0];
            BaseLanguage baseLanguage = Language.BaseLanguages[0];

            // Change internal dictionary name to user-defined
            baseLanguage.InternalName = "SampleBaseLanguage";

            //set custom doctionary for base language
            setDictionary(language, baseLanguage, dictionary, languageWeight, dictionaryWeight);

            return Language;
        }

        private void setDictionary(string language, BaseLanguage baseLanguage, List<string> dictionary, int languageWeight, int dictionaryWeight)
        {
            throw new NotImplementedException();
            Debug.Assert(false); //TODO CASE CREATED 5062
           // string path = Path.Combine(SettingsContainer.Instance.Directories[Infrastructure.Source.UserDocsDir].FullName, "CustomDictionary.amd"); //TODO CASE CREATED
            //create dictionary file
            //makeDictionary(
            //    path,
            //    baseLanguage,
            //    dictionary);
            // Get collection of dictionary descriptions and remove all items
            //DictionaryDescriptions dictionaryDescriptions = baseLanguage.DictionaryDescriptions;
            //if (language == null)
            //{
            //    dictionaryDescriptions.DeleteAll();
            //}
            //// lower weight of defoult dictionary (defoult weight = 100)
            //foreach (DictionaryDescription d in dictionaryDescriptions)
            //{
            //    d.Weight = languageWeight;
            //}

            //// Create user dictionary description and add it to the collection
            //IDictionaryDescription dictionaryDescription = dictionaryDescriptions.AddNew(DictionaryTypeEnum.DT_UserDictionary);
            //UserDictionaryDescription userDictionaryDescription = dictionaryDescription.GetAsUserDictionaryDescription();
            //userDictionaryDescription.Weight = dictionaryWeight;
            //userDictionaryDescription.FileName = path;
        }

        private void makeDictionary(string path, BaseLanguage baseLanguage, List<string> words)
        {
            LanguageDatabase languageDatabase = Engine.CreateLanguageDatabase();
            Dictionary dictionary = languageDatabase.CreateNewDictionary(path,
                baseLanguage.LanguageId);
            dictionary.Name = "CustomDictionary";

            // Add words to dictionary
            foreach (string word in words)
            {
                dictionary.AddWord(word, 100);
            }
        }

        private void SetPageAnalyzeParameters(PageProcessingParams pageProcessingParams)
        {
            PageAnalysisParams pageAnalysisParams = pageProcessingParams.PageAnalysisParams ?? engine.CreatePageAnalysisParams();

            SetTableAnalyzeParameters(pageAnalysisParams);
        }

        private void SetTableAnalyzeParameters(PageAnalysisParams pageAnalysisParams)
        {
            TableAnalysisParams tableAnalysisParams = pageAnalysisParams.TableAnalysisParams ?? engine.CreateTableAnalysisParams();
            pageAnalysisParams.TableAnalysisParams = tableAnalysisParams;
        }

        private void SetObjectExtractionParameters(PageProcessingParams pageProcessingParams)
        {
            ObjectsExtractionParams objectExtracionParams = pageProcessingParams.ObjectsExtractionParams ?? engine.CreateObjectsExtractionParams();
            pageProcessingParams.ObjectsExtractionParams = objectExtracionParams;
        }

        private WritingStyleEnum GetWritingSyle(string language)
        {
            WritingStyleEnum writingStyle;
            if (!Enum.TryParse<WritingStyleEnum>($"WS_{language}", out writingStyle))
                writingStyle = WritingStyleEnum.WS_British;
            return writingStyle;
        }

        public void Dispose()
        {
            foreach (FineReaderDocument doc in documents.Values)
            {
                try
                {
                    doc.Document.Close();
                }
                catch { }
            }

            try
            {
#if DEBUG
                long memorySizeBefore = GC.GetTotalMemory(false);
#endif
                lock (syncRoot)
                {
                    engine = null;
                    instance = null;
                    engineLoader.ExplicitlyUnload();
                    engineLoader = null;
                    GC.Collect();
                    System.Threading.Thread.Sleep(1);
                    GC.WaitForPendingFinalizers();
                }
#if DEBUG
                long memorySizeAfter = GC.GetTotalMemory(false);
#endif
            }
            catch { }
        }
    }
}
