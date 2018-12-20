using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Lucene.Net.Store;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace WinSearch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
    public class Indexer
    {
        private IndexWriter writer;
        public Indexer(string path)
        {
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            Directory directory = new RAMDirectory();
            IndexWriter.MaxFieldLength maxFieldLength = new IndexWriter.MaxFieldLength(10000);
            Directory d = new RAMDirectory();
            writer = new IndexWriter(d, analyzer, maxFieldLength);
            Document doc = new Document();
            doc.Add(new Field("Sentence", "刘备", Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);

            Document document2 = new Document();
            document2.Add(new Field("Sentence", "张飞", Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(document2);

            Document document3 = new Document();
            document3.Add(new Field("Sentence", "关羽", Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(document3);

            writer.Optimize();
        }

        public void Show()
        {

            using (IndexSearcher searcher = new IndexSearcher(directory))
            {
                Term t = new Term("Sentence", "飞");
                Query query = new TermQuery(t);
                TopDocs docs = searcher.Search(query, null, 1000);
                Console.WriteLine(docs.TotalHits);
                Console.WriteLine(docs.ScoreDocs[0].Doc);
                Document doc = searcher.Doc(docs.ScoreDocs[0].Doc);
                Console.WriteLine(doc.Get("Sentence"));
            }

        }
    }
}
