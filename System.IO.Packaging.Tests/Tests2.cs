using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;

// packages\xunit.runner.console.2.0.0\tools\xunit.console System.IO.Packaging.Tests\bin\Debug\System.IO.Packaging.Tests.dll

namespace System.IO.Packaging.Tests
{
    public class Tests2
    {
        [Fact]
        public void T201_FileFormatException()
        {
            var e = new FileFormatException();
            Assert.NotNull(e);
        }

        [Fact]
        public void T202_FileFormatException()
        {
            var e2 = new IOException("Test");
            var e = new FileFormatException("Test", e2);
            Assert.NotNull(e);
        }

        [Fact]
        public void T203_FileFormatException()
        {
            var partUri = new Uri("/idontexist.xml", UriKind.Relative);
            var e = new FileFormatException(partUri);
            Assert.NotNull(e);
        }

        [Fact]
        public void T203A_FileFormatException()
        {
            Uri partUri = null;
            var e = new FileFormatException(partUri);
            Assert.NotNull(e);
        }

        [Fact]
        public void T204_FileFormatException()
        {
            var partUri = new Uri("/idontexist.xml", UriKind.Relative);
            var e = new FileFormatException(partUri, "Test");
            Assert.NotNull(e);
        }

        [Fact]
        public void T205_FileFormatException()
        {
            var partUri = new Uri("/idontexist.xml", UriKind.Relative);
            var e2 = new IOException("Test");
            var e = new FileFormatException(partUri, e2);
            Assert.NotNull(e);
        }

        [Fact]
        public void T205A_FileFormatException()
        {
            Uri partUri = null;
            var e2 = new IOException("Test");
            var e = new FileFormatException(partUri, e2);
            Assert.NotNull(e);
        }

        [Fact]
        public void T206_FileFormatException()
        {
            var partUri = new Uri("/idontexist.xml", UriKind.Relative);
            var e2 = new IOException("Test");
            var e = new FileFormatException(partUri, "Test", e2);
            Assert.NotNull(e);
        }

        [Fact]
        public void T207_Invalid_Rel_Type()
        {
            var docName = "invalid_rel_type.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
            {
                Assert.Throws<FileFormatException>(() =>
                {
                    PackageRelationship docPackageRelationship4 =
                                    package
                                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                                    .FirstOrDefault();
                });
            }
            fiGuidName.Delete();
        }

        [Fact]
        public void T208_InvalidParameter()
        {
            var docName = "plain.docx";
            var ba = TestFileLib.GetByteArray(docName);
            var documentPath = "document.xml";
            Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(documentPath, UriKind.Relative));

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(ba, 0, ba.Length);
                Package package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
                PackagePart packagePartDocument = null;
                Assert.Throws<ArgumentException>(() => { packagePartDocument = package.CreatePart(partUriDocument, "image/jpeg; prop= ;"); });
            }
        }

        [Fact]
        public void T209_NullContentType()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var c = new ContentType(null);
            });
        }

        [Fact]
        public void T210_EmptyStringContentType()
        {
            var c = new ContentType("");
        }

        [Fact]
        public void T211_QuotedText()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var c = new ContentType("image/jpeg; prop=\"   value   \"    ; prop2=value2\"");
            });
        }

        [Fact]
        public void T212_OpenDocumentWithExternalRelationship()
        {
            var docName = "ExternalLink.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
            {
                PackageRelationship docPackageRelationship4 = package
                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                    .FirstOrDefault();

                Uri documentUri = PackUriHelper
                    .ResolvePartUri(
                       new Uri("/", UriKind.Relative),
                             docPackageRelationship4.TargetUri);

                var mainDocumentPart = package.GetPart(documentUri);

                var rel = mainDocumentPart
                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                    .FirstOrDefault();
            }

            //fiGuidName.Delete();
        }

        [Fact]
        public void T213_InvalidContentTypes1()
        {
            var docName = "InvalidContentTypes1.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            Assert.Throws<XmlException>(() =>
            {
                using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
                {
                }
            });
        }

        [Fact]
        public void T214_InvalidContentTypes2()
        {
            var docName = "InvalidContentTypes2.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            Assert.Throws<XmlException>(() =>
            {
                using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
                {
                }
            });
        }

        [Fact]
        public void T215_GetNormalizedPartUri()
        {
            var docName = "ExternalLink.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
            {
                PackageRelationship docPackageRelationship4 = package
                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                    .FirstOrDefault();

                Uri documentUri = PackUriHelper
                    .ResolvePartUri(
                       new Uri("/", UriKind.Relative),
                             docPackageRelationship4.TargetUri);

                Uri normalizedUri = PackUriHelper.GetNormalizedPartUri(documentUri);
            }

            fiGuidName.Delete();
        }

        [Fact]
        public void T216_GetNormalizedPartUri()
        {
            var docName = "ExternalLink.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
            {
                PackageRelationship docPackageRelationship4 = package
                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                    .FirstOrDefault();

                Uri documentUri = PackUriHelper
                    .ResolvePartUri(
                       new Uri("/", UriKind.Relative),
                             docPackageRelationship4.TargetUri);

                Assert.Throws<ArgumentNullException>(() =>
                {
                    Uri normalizedUri = PackUriHelper.GetNormalizedPartUri(null);
                });
            }

            fiGuidName.Delete();
        }

        [Fact]
        public void T217_ComparePartUri()
        {
            var docName = "ExternalLink.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
            {
                PackageRelationship docPackageRelationship4 = package
                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                    .FirstOrDefault();

                Uri documentUri = PackUriHelper
                    .ResolvePartUri(
                       new Uri("/", UriKind.Relative),
                             docPackageRelationship4.TargetUri);

                var otherUri = new Uri("/idontexist.xml", UriKind.Relative);

                var r = PackUriHelper.ComparePartUri(documentUri, otherUri);
                Assert.Equal(14, r);
            }

            fiGuidName.Delete();
        }

        [Fact]
        public void T218_ComparePartUri()
        {
            var docName = "ExternalLink.docx";
            var fiGuidName = TestFileLib.GetFileSavedWithGuidName(docName);

            using (Package package = Package.Open(fiGuidName.FullName, FileMode.Open))
            {
                PackageRelationship docPackageRelationship4 = package
                    .GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument")
                    .FirstOrDefault();

                Uri documentUri = PackUriHelper
                    .ResolvePartUri(
                       new Uri("/", UriKind.Relative),
                             docPackageRelationship4.TargetUri);

                var r = PackUriHelper.ComparePartUri(documentUri, null);
                Assert.Equal(1, r);
                r = PackUriHelper.ComparePartUri(null, documentUri);
                Assert.Equal(-1, r);
            }

            fiGuidName.Delete();
        }

        [Fact]
        public void T300_XmlCompatibilityReader()
        {
            string xml = @"<root></root>";
            using (StringReader sr = new StringReader(xml))
            using (XmlReader xr = XmlReader.Create(sr))
            using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
            {
                //Skips over the following - ProcessingInstruction, DocumentType, Comment, Whitespace, or SignificantWhitespace
                //If the reader is currently at a content node then this function call is a no-op
                var r = reader.MoveToContent();
                var l = reader.LocalName;
                while (reader.Read())
                {
                    reader.MoveToContent();
                }
            }
        }

        [Fact]
        public void T301_XmlCompatibilityReader()
        {
            string xml = @"<root xmlns='http://old'></root>";
            using (StringReader sr = new StringReader(xml))
            using (XmlReader xr = XmlReader.Create(sr))
            using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
            {
                reader.DeclareNamespaceCompatibility("http://new", "http://old");
                var r = reader.MoveToContent();
                var l = reader.LocalName;
                while (reader.Read())
                {
                    reader.MoveToContent();
                }
            }
        }

        [Fact]
        public void T302_XmlCompatibilityReader()
        {
            string xml = @"<root xmlns='http://old'><child></child></root>";
            using (StringReader sr = new StringReader(xml))
            using (XmlReader xr = XmlReader.Create(sr))
            using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
            {
                reader.DeclareNamespaceCompatibility("http://new", "http://old");
                reader.DeclareNamespaceCompatibility("http://new2", "http://old");
                var r = reader.MoveToContent();
                var l = reader.LocalName;
                while (reader.Read())
                {
                    reader.MoveToContent();
                }
            }
        }

        [Fact]
        public void T303_XmlCompatibilityReader()
        {
            string xml = @"<root xmlns='http://old'><child></child></root>";
            using (StringReader sr = new StringReader(xml))
            using (XmlReader xr = XmlReader.Create(sr))
            using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
            {
                reader.DeclareNamespaceCompatibility("http://new", "http://old");
                reader.DeclareNamespaceCompatibility("http://new2", "http://new");
                var r = reader.MoveToContent();
                var l = reader.LocalName;
                while (reader.Read())
                {
                    reader.MoveToContent();
                }
            }
        }

        [Fact]
        public void T304_XmlCompatibilityReader()
        {
            string xml =
@"<root xmlns='http://old' xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006'>
  <mc:AlternateContent>
    <mc:Choice>
      <p/>
    </mc:Choice>
    <mc:Fallback>
      <p/>
    </mc:Fallback>
  </mc:AlternateContent>
</root>";
            Assert.Throws<XmlException>(() =>
            {
                using (StringReader sr = new StringReader(xml))
                using (XmlReader xr = XmlReader.Create(sr))
                using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
                {
                    reader.DeclareNamespaceCompatibility("http://new", "http://old");
                    reader.DeclareNamespaceCompatibility("http://new2", "http://new");
                    var r = reader.MoveToContent();
                    var l = reader.LocalName;
                    while (reader.Read())
                    {
                        reader.MoveToContent();
                    }
                }
            });
        }

        [Fact]
        public void T305_XmlCompatibilityReader()
        {
            string xml =
@"<root xmlns='http://old' xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006' xmlns:cx1='http://cx1'>
  <mc:AlternateContent>
    <mc:Choice Requires='cx1'>
      <p/>
    </mc:Choice>
    <mc:Fallback>
      <p/>
    </mc:Fallback>
  </mc:AlternateContent>
</root>";

            using (StringReader sr = new StringReader(xml))
            using (XmlReader xr = XmlReader.Create(sr))
            using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
            {
                var r = reader.MoveToContent();
                var l = reader.LocalName;
                while (reader.Read())
                {
                    reader.MoveToContent();
                }
            }
        }

        [Fact]
        public void T306_XmlCompatibilityReader()
        {
            string xml =
@"<root xmlns='http://old' xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006' xmlns:cx1='http://cx1'>
    <mc:Choice Requires='cx1'>
      <p/>
    </mc:Choice>
    <mc:Fallback>
      <p/>
    </mc:Fallback>
</root>";

            Assert.Throws<XmlException>(() =>
                {
                    using (StringReader sr = new StringReader(xml))
                    using (XmlReader xr = XmlReader.Create(sr))
                    using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
                    {
                        var r = reader.MoveToContent();
                        var l = reader.LocalName;
                        while (reader.Read())
                        {
                            reader.MoveToContent();
                        }
                    }
                });
        }

        [Fact]
        public void T307_XmlCompatibilityReader()
        {
            string xml =
@"<root xmlns='http://old' xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006' xmlns:cx1='http://cx1'>
  <mc:AlternateContent>
    <mc:Fallback>
      <p/>
    </mc:Fallback>
    <mc:Choice Requires='cx1'>
      <p/>
    </mc:Choice>
  </mc:AlternateContent>
</root>";

            Assert.Throws<XmlException>(() =>
                {
                    using (StringReader sr = new StringReader(xml))
                    using (XmlReader xr = XmlReader.Create(sr))
                    using (XmlCompatibilityReader reader = new XmlCompatibilityReader(xr))
                    {
                        var r = reader.MoveToContent();
                        var l = reader.LocalName;
                        while (reader.Read())
                        {
                            reader.MoveToContent();
                        }
                    }
                });
        }

    }
}
