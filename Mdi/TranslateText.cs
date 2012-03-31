using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordNote.BingTranslate;

namespace WordNote
{
    class TranslateText
    {
        public string Translate(string Source, string ToLanguage)
        {
            try
            {
                if (ToLanguage == string.Empty)
                    return Source;
                BingTranslate.Translation TranslateText = new BingTranslate.Translation();
                BingTranslate.LanguageServiceClient TranslateClient = new BingTranslate.LanguageServiceClient();
                TranslateText.OriginalText = Source;
                string OriginalLanguage = TranslateClient.Detect(myInfo.AppID, Source);
                TranslateText.TranslatedText = TranslateClient.Translate(myInfo.AppID, Source, OriginalLanguage, ToLanguage, "text/plain", "general");
                return TranslateText.TranslatedText;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return string.Empty;
            }
        }

        

    }
}
