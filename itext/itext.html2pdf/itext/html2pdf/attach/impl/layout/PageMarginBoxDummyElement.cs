using System;
using System.Collections.Generic;
using iText.IO.Util;
using iText.StyledXmlParser.Css.Page;
using iText.StyledXmlParser.Node;

namespace iText.Html2pdf.Attach.Impl.Layout {
    [System.ObsoleteAttribute(@"Remove this class in 7.2 and use iText.StyledXmlParser.Css.Page.PageMarginBoxContextNode instead (by making it implement iText.StyledXmlParser.Node.IElementNode )."
        )]
    internal class PageMarginBoxDummyElement : IElementNode, ICustomElementNode {
        /// <summary>The resolved styles.</summary>
        private IDictionary<String, String> elementResolvedStyles;

        public virtual String Name() {
            return PageMarginBoxContextNode.PAGE_MARGIN_BOX_TAG;
        }

        public virtual IAttributes GetAttributes() {
            throw new NotSupportedException();
        }

        public virtual String GetAttribute(String key) {
            throw new NotSupportedException();
        }

        public virtual IList<IDictionary<String, String>> GetAdditionalHtmlStyles() {
            throw new NotSupportedException();
        }

        public virtual void AddAdditionalHtmlStyles(IDictionary<String, String> styles) {
            throw new NotSupportedException();
        }

        public virtual String GetLang() {
            throw new NotSupportedException();
        }

        public virtual IList<INode> ChildNodes() {
            throw new NotSupportedException();
        }

        public virtual void AddChild(INode node) {
            throw new NotSupportedException();
        }

        public virtual INode ParentNode() {
            throw new NotSupportedException();
        }

        public virtual void SetStyles(IDictionary<String, String> stringStringMap) {
            elementResolvedStyles = stringStringMap;
        }

        public virtual IDictionary<String, String> GetStyles() {
            return elementResolvedStyles == null ? JavaCollectionsUtil.EmptyMap<String, String>() : elementResolvedStyles;
        }
    }
}
