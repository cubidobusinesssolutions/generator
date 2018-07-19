using System;

namespace cubido.OpenApi.Angular.Template
{
    public class ConvertOptions
    {
        // see https://github.com/ocombe/ocLazyLoad/issues/239
        /// <summary>Relative environment path</summary>
        [Obsolete("Use HttpInterceptor base url instead")]
        public string BaseUrl { get; set; } = null;
        internal bool HasBaseUrl { get => !string.IsNullOrEmpty(BaseUrl); }
        public string ModelsDirectoryName { get; set; }
        public string ServicesDirectoryName { get; set; }
        public string HelpersDirectoryName { get; set; }

        public bool IncludeGenerationTimestamp { get; set; } = false;
        public bool DoubleQuotes { get; set; } = false;


        public string ModelsRelativePath
        {
            get
            {
                Uri path1 = new Uri(@"c:\" + ServicesDirectoryName + "\\");
                Uri path2 = new Uri(@"c:\" + ModelsDirectoryName + "\\");
                Uri diff = path1.MakeRelativeUri(path2);
                return diff.OriginalString;
            }
        }
    }
}
