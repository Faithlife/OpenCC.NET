using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace OpenCCNET
{
    public static partial class ZhConverter
    {
        public static class ZhDictionary
        {
            /// <summary>
            /// 简体中文=>繁体中文（OpenCC标准）单字转换字典
            /// </summary>
            public static IDictionary<string, string> STCharacters => _STCharacters.Value;

            /// <summary>
            /// 简体中文=>繁体中文（OpenCC标准）词汇转换字典
            /// </summary>
            public static IDictionary<string, string> STPhrases => _STPhrases.Value;

            /// <summary>
            /// 繁体中文（OpenCC标准）=>简体中文单字转换字典
            /// </summary>
            public static IDictionary<string, string> TSCharacters => _TSCharacters.Value;

            /// <summary>
            /// 繁体中文（OpenCC标准）=>简体中文词汇转换字典
            /// </summary>
            public static IDictionary<string, string> TSPhrases => _TSPhrases.Value;

            /// <summary>
            /// 繁体中文（OpenCC标准）=>繁体中文（台湾）单字转换字典
            /// </summary>
            public static IDictionary<string, string> TWVariants => _TWVariants.Value;

            /// <summary>
            /// 繁体中文（OpenCC标准）=>繁体中文（台湾）词汇转换字典
            /// </summary>
            public static IDictionary<string, string> TWPhrases => _TWPhrases.Value;

            /// <summary>
            /// 繁体中文（台湾）=>繁体中文（OpenCC标准）单字转换字典
            /// </summary>
            public static IDictionary<string, string> TWVariantsRev => _TWVariantsRev.Value;

            /// <summary>
            /// 繁体中文（台湾）=>繁体中文（OpenCC标准）异体字词汇转换字典
            /// </summary>
            public static IDictionary<string, string> TWVariantsRevPhrases => _TWVariantsRevPhrases.Value;

            /// <summary>
            /// 繁体中文（台湾）=>繁体中文（OpenCC标准）词汇转换字典
            /// </summary>
            public static IDictionary<string, string> TWPhrasesRev => _TWPhrasesRev.Value;

            /// <summary>
            /// 繁体中文（OpenCC标准）=>繁体中文（香港）单字转换字典
            /// </summary>
            public static IDictionary<string, string> HKVariants => _HKVariants.Value;

            /// <summary>
            /// 繁体中文（香港）=>繁体中文（OpenCC标准）单字转换字典
            /// </summary>
            public static IDictionary<string, string> HKVariantsRev => _HKVariantsRev.Value;

            /// <summary>
            /// 繁体中文（香港）=>繁体中文（OpenCC标准）异体字词汇转换字典
            /// </summary>
            public static IDictionary<string, string> HKVariantsRevPhrases => _HKVariantsRevPhrases.Value;

            /// <summary>
            /// 日语（旧字体）=>日语（新字体）单字转换字典
            /// </summary>
            public static IDictionary<string, string> JPVariants => _JPVariants.Value;

            /// <summary>
            /// 日语（新字体）=>日语（旧字体）单字转换字典
            /// </summary>
            public static IDictionary<string, string> JPVariantsRev => _JPVariantsRev.Value;

            /// <summary>
            /// 日语（新字体）=>日语（旧字体）异体字单字转换字典
            /// </summary>
            public static IDictionary<string, string> JPShinjitaiCharacters => _JPShinjitaiCharacters.Value;

            /// <summary>
            /// 日语（新字体）=>日语（旧字体）异体字词汇转换字典
            /// </summary>
            public static IDictionary<string, string> JPShinjitaiPhrases => _JPShinjitaiPhrases.Value;


            static ZhDictionary()
            {
                _STCharacters = new(() => LoadDictionary("STCharacters"));
                _STPhrases = new(() => LoadDictionary("STPhrases"));
                _TSCharacters = new(() => LoadDictionary("TSCharacters"));
                _TSPhrases = new(() => LoadDictionary("TSPhrases"));
                _TWVariants = new(() => LoadDictionary("TWVariants"));
                _TWPhrases = new(() => LoadDictionary("TWPhrasesIT", "TWPhrasesName", "TWPhrasesOther"));
                _TWVariantsRev = new(() => LoadDictionaryReversed("TWVariants"));
                _TWVariantsRevPhrases = new(() => LoadDictionary("TWVariantsRevPhrases"));
                _TWPhrasesRev = new(() => LoadDictionaryReversed("TWPhrasesIT", "TWPhrasesName", "TWPhrasesOther"));
                _HKVariants = new(() => LoadDictionary("HKVariants"));
                _HKVariantsRev = new(() => LoadDictionaryReversed("HKVariants"));
                _HKVariantsRevPhrases = new(() => LoadDictionary("HKVariantsRevPhrases"));
                _JPVariants = new(() => LoadDictionary("JPVariants"));
                _JPVariantsRev = new(() => LoadDictionaryReversed("JPVariants"));
                _JPShinjitaiCharacters = new(() => LoadDictionary("JPShinjitaiCharacters"));
                _JPShinjitaiPhrases = new(() => LoadDictionary("JPShinjitaiPhrases"));
            }

            /// <summary>
            /// 加载字典文件
            /// </summary>
            /// <param name="dictionaryNames">字典名称</param>
            private static IDictionary<string, string> LoadDictionary(params string[] dictionaryNames)
            {
                using var zipArchive = new ZipArchive(typeof(ZhDictionary).Assembly.GetManifestResourceStream("Dictionary.zip") ?? throw new InvalidOperationException("Missing dictionary zip file."));
                var dictionaryPaths = dictionaryNames.Select(name => $"{name}.txt")
                    .ToList();
                var dictionary = new Dictionary<string, string>();
                foreach (var path in dictionaryPaths)
                {
                    using (var sr = new StreamReader(zipArchive.GetEntry(path)?.Open() ?? throw new InvalidOperationException($"Missing dictionary {path}.")))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            var items = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                            dictionary[items[0]] = items[1];
                        }
                    }
                }

                return dictionary;
            }

            /// <summary>
            /// 反向加载字典文件
            /// </summary>
            /// <param name="dictionaryNames">字典名称</param>
            private static IDictionary<string, string> LoadDictionaryReversed(params string[] dictionaryNames)
            {
                using var zipArchive = new ZipArchive(typeof(ZhDictionary).Assembly.GetManifestResourceStream("Dictionary.zip") ?? throw new InvalidOperationException("Missing dictionary zip file."));
                var dictionaryPaths = dictionaryNames.Select(name => $"{name}.txt");
                var dictionary = new Dictionary<string, string>();
                foreach (var path in dictionaryPaths)
                {
                    using (var sr = new StreamReader(zipArchive.GetEntry(path)?.Open() ?? throw new InvalidOperationException($"Missing dictionary {path}.")))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            var items = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                            for (var i = 1; i < items.Length; i++)
                            {
                                dictionary[items[i]] = items[0];
                            }
                        }
                    }
                }

                return dictionary;
            }

            private static readonly Lazy<IDictionary<string, string>> _STCharacters;
            private static readonly Lazy<IDictionary<string, string>> _STPhrases;
            private static readonly Lazy<IDictionary<string, string>> _TSCharacters;
            private static readonly Lazy<IDictionary<string, string>> _TSPhrases;
            private static readonly Lazy<IDictionary<string, string>> _TWVariants;
            private static readonly Lazy<IDictionary<string, string>> _TWPhrases;
            private static readonly Lazy<IDictionary<string, string>> _TWVariantsRev;
            private static readonly Lazy<IDictionary<string, string>> _TWVariantsRevPhrases;
            private static readonly Lazy<IDictionary<string, string>> _TWPhrasesRev;
            private static readonly Lazy<IDictionary<string, string>> _HKVariants;
            private static readonly Lazy<IDictionary<string, string>> _HKVariantsRev;
            private static readonly Lazy<IDictionary<string, string>> _HKVariantsRevPhrases;
            private static readonly Lazy<IDictionary<string, string>> _JPVariants;
            private static readonly Lazy<IDictionary<string, string>> _JPVariantsRev;
            private static readonly Lazy<IDictionary<string, string>> _JPShinjitaiCharacters;
            private static readonly Lazy<IDictionary<string, string>> _JPShinjitaiPhrases;
        }
    }
}