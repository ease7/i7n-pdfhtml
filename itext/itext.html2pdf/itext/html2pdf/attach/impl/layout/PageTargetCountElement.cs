/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace iText.Html2pdf.Attach.Impl.Layout {
    /// <summary>
    /// <see cref="iText.Layout.Element.Text"/>
    /// implementation to be used for the page target-counter.
    /// </summary>
    public class PageTargetCountElement : Text {
        private readonly String target;

        /// <summary>
        /// Instantiates a new
        /// <see cref="PageTargetCountElement"/>.
        /// </summary>
        /// <param name="target">name of the corresponding target</param>
        public PageTargetCountElement(String target)
            : base("1234567890") {
            this.target = target.Replace("'", "").Replace("#", "");
        }

        /// <summary>Gets element's target.</summary>
        /// <returns>target which was specified for this element.</returns>
        public virtual String GetTarget() {
            return target;
        }

        /// <summary><inheritDoc/></summary>
        protected override IRenderer MakeNewRenderer() {
            return new PageTargetCountRenderer(this);
        }
    }
}
