using System;
using System.IO;
using Org.Jsoup;
using Org.Jsoup.Nodes;
using Org.Jsoup.Select;

namespace Org.Jsoup.Integration {
    /// <summary>Integration test: parses from real-world example HTML.</summary>
    /// <author>Jonathan Hedley, jonathan@hedley.net</author>
    public class ParseTest {
        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestSmhBizArticle() {
            FileInfo @in = PortTestUtil.GetFile("/htmltests/smh-biz-article-1.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, "UTF-8", "http://www.smh.com.au/business/the-boards-next-fear-the-female-quota-20100106-lteq.html"
                );
            NUnit.Framework.Assert.AreEqual("The board’s next fear: the female quota", doc.Title());
            // note that the apos in the source is a literal ’ (8217), not escaped or '
            NUnit.Framework.Assert.AreEqual("en", doc.Select("html").Attr("xml:lang"));
            Elements articleBody = doc.Select(".articleBody > *");
            NUnit.Framework.Assert.AreEqual(17, articleBody.Count);
        }

        // todo: more tests!
        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestNewsHomepage() {
            FileInfo @in = PortTestUtil.GetFile("/htmltests/news-com-au-home.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, "UTF-8", "http://www.news.com.au/");
            NUnit.Framework.Assert.AreEqual("News.com.au | News from Australia and around the world online | NewsComAu"
                , doc.Title());
            NUnit.Framework.Assert.AreEqual("Brace yourself for Metro meltdown", doc.Select(".id1225817868581 h4").Text
                ().Trim());
            Element a = doc.Select("a[href=/entertainment/horoscopes]").First();
            NUnit.Framework.Assert.AreEqual("/entertainment/horoscopes", a.Attr("href"));
            NUnit.Framework.Assert.AreEqual("http://www.news.com.au/entertainment/horoscopes", a.Attr("abs:href"));
            Element hs = doc.Select("a[href*=naughty-corners-are-a-bad-idea]").First();
            NUnit.Framework.Assert.AreEqual("http://www.heraldsun.com.au/news/naughty-corners-are-a-bad-idea-for-kids/story-e6frf7jo-1225817899003"
                , hs.Attr("href"));
            NUnit.Framework.Assert.AreEqual(hs.Attr("href"), hs.Attr("abs:href"));
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestGoogleSearchIpod() {
            FileInfo @in = PortTestUtil.GetFile("/htmltests/google-ipod.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, "UTF-8", "http://www.google.com/search?hl=en&q=ipod&aq=f&oq=&aqi=g10"
                );
            NUnit.Framework.Assert.AreEqual("ipod - Google Search", doc.Title());
            Elements results = doc.Select("h3.r > a");
            NUnit.Framework.Assert.AreEqual(12, results.Count);
            NUnit.Framework.Assert.AreEqual("http://news.google.com/news?hl=en&q=ipod&um=1&ie=UTF-8&ei=uYlKS4SbBoGg6gPf-5XXCw&sa=X&oi=news_group&ct=title&resnum=1&ved=0CCIQsQQwAA"
                , results[0].Attr("href"));
            NUnit.Framework.Assert.AreEqual("http://www.apple.com/itunes/", results[1].Attr("href"));
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestBinary() {
            FileInfo @in = PortTestUtil.GetFile("/htmltests/thumb.jpg");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, "UTF-8");
            // nothing useful, but did not blow up
            NUnit.Framework.Assert.IsTrue(doc.Text().Contains("gd-jpeg"));
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestYahooJp() {
            FileInfo @in = PortTestUtil.GetFile("/htmltests/yahoo-jp.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, "UTF-8", "http://www.yahoo.co.jp/index.html");
            // http charset is utf-8.
            NUnit.Framework.Assert.AreEqual("Yahoo! JAPAN", doc.Title());
            Element a = doc.Select("a[href=t/2322m2]").First();
            NUnit.Framework.Assert.AreEqual("http://www.yahoo.co.jp/_ylh=X3oDMTB0NWxnaGxsBF9TAzIwNzcyOTYyNjUEdGlkAzEyBHRtcGwDZ2Ex/t/2322m2"
                , a.Attr("abs:href"));
            // session put into <base>
            NUnit.Framework.Assert.AreEqual("全国、人気の駅ランキング", a.Text());
        }

        private const String newsHref = "http://news.baidu.com/";

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestBaidu() {
            // tests <meta http-equiv="Content-Type" content="text/html;charset=gb2312">
            FileInfo @in = PortTestUtil.GetFile("/htmltests/baidu-cn-home.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://www.baidu.com");
            // http charset is gb2312, but NOT specifying it, to test http-equiv parse
            Element submit = doc.Select("#su").First();
            NUnit.Framework.Assert.AreEqual("百度一下", submit.Attr("value"));
            // test from attribute match
            submit = doc.Select("input[value=百度一下]").First();
            NUnit.Framework.Assert.AreEqual("su", submit.Id());
            Element newsLink = doc.Select("a:contains(新)").First();
            NUnit.Framework.Assert.AreEqual(newsHref, newsLink.AbsUrl("href"));
            // check auto-detect from meta
            NUnit.Framework.Assert.AreEqual("GB2312", doc.OutputSettings().Charset().DisplayName());
            NUnit.Framework.Assert.AreEqual("<title>百度一下，你就知道      </title>", doc.Select("title").OuterHtml());
            doc.OutputSettings().Charset("ascii");
            NUnit.Framework.Assert.AreEqual("<title>&#x767e;&#x5ea6;&#x4e00;&#x4e0b;&#xff0c;&#x4f60;&#x5c31;&#x77e5;&#x9053;      </title>"
                , doc.Select("title").OuterHtml());
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestBaiduVariant() {
            // tests <meta charset> when preceded by another <meta>
            FileInfo @in = PortTestUtil.GetFile("/htmltests/baidu-variant.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://www.baidu.com/");
            // http charset is gb2312, but NOT specifying it, to test http-equiv parse
            // check auto-detect from meta
            NUnit.Framework.Assert.AreEqual("GB2312", doc.OutputSettings().Charset().DisplayName());
            NUnit.Framework.Assert.AreEqual("<title>百度一下，你就知道</title>", doc.Select("title").OuterHtml());
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestHtml5Charset() {
            // test that <meta charset="gb2312"> works
            FileInfo @in = PortTestUtil.GetFile("/htmltests/meta-charset-1.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://example.com/");
            //gb2312, has html5 <meta charset>
            NUnit.Framework.Assert.AreEqual("新", doc.Text());
            NUnit.Framework.Assert.AreEqual("GB2312", doc.OutputSettings().Charset().DisplayName());
            // double check, no charset, falls back to utf8 which is incorrect
            @in = PortTestUtil.GetFile("/htmltests/meta-charset-2.html");
            //
            doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://example.com");
            // gb2312, no charset
            NUnit.Framework.Assert.AreEqual("UTF-8", doc.OutputSettings().Charset().DisplayName());
            NUnit.Framework.Assert.IsFalse("新".Equals(doc.Text()));
            // confirm fallback to utf8
            @in = PortTestUtil.GetFile("/htmltests/meta-charset-3.html");
            doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://example.com/");
            // utf8, no charset
            NUnit.Framework.Assert.AreEqual("UTF-8", doc.OutputSettings().Charset().DisplayName());
            NUnit.Framework.Assert.AreEqual("新", doc.Text());
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestBrokenHtml5CharsetWithASingleDoubleQuote() {
            Stream @in = InputStreamFrom("<html>\n" + "<head><meta charset=UTF-8\"></head>\n" + "<body></body>\n" + "</html>"
                );
            Document doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://example.com/");
            NUnit.Framework.Assert.AreEqual("UTF-8", doc.OutputSettings().Charset().DisplayName());
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestNytArticle() {
            // has tags like <nyt_text>
            FileInfo @in = PortTestUtil.GetFile("/htmltests/nyt-article-1.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, null, "http://www.nytimes.com/2010/07/26/business/global/26bp.html?hp"
                );
            Element headline = doc.Select("nyt_headline[version=1.0]").First();
            NUnit.Framework.Assert.AreEqual("As BP Lays Out Future, It Will Not Include Hayward", headline.Text());
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void TestYahooArticle() {
            FileInfo @in = PortTestUtil.GetFile("/htmltests/yahoo-article-1.html");
            Document doc = Org.Jsoup.Jsoup.Parse(@in, "UTF-8", "http://news.yahoo.com/s/nm/20100831/bs_nm/us_gm_china"
                );
            Element p = doc.Select("p:contains(Volt will be sold in the United States").First();
            NUnit.Framework.Assert.AreEqual("In July, GM said its electric Chevrolet Volt will be sold in the United States at $41,000 -- $8,000 more than its nearest competitor, the Nissan Leaf."
                , p.Text());
        }

        public static Stream InputStreamFrom(String s) {
            try {
                return new MemoryStream(s.GetBytes("UTF-8"));
            }
            catch (ArgumentException e) {
                throw new Exception("Unsupported encoding", e);
            }
        }
    }
}