using System;
using iText.Html2pdf.Css.W3c;
using iText.Test.Attributes;

namespace iText.Html2pdf.Css.W3c.Css_backgrounds.Reference {
    [LogMessage(iText.IO.LogMessageConstant.OCCUPIED_AREA_HAS_NOT_BEEN_INITIALIZED, Count = 8)]
    public class Css3BorderImageRepeatRepeatRefTest : W3CCssTest {
        protected internal override String GetHtmlFileName() {
            return "css3-border-image-repeat-repeat-ref.html";
        }
    }
}