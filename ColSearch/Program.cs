using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            //分析对象
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            //路径对象（索引的存储位置）
            Directory directory = new RAMDirectory();//索引创建于内存
            //FSDirectory directory = FSDirectory.Open(@"C:\Users\24900\Desktop\log");//索引写入到本地
            //索引的最大字段长度
            IndexWriter.MaxFieldLength maxFieldLength = new IndexWriter.MaxFieldLength(10000);

            //写入数据到索引
            using (IndexWriter writer = new IndexWriter(directory, analyzer, maxFieldLength))
            {
                //文档对象 可以理解为一篇新闻
                Document document1 = new Document();
                //"刘备" 新闻的标题或者内容 
                document1.Add(new Field("Sentence", "刘备", Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(document1);

                Document document2 = new Document();
                document2.Add(new Field("Sentence", "张飞", Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(document2);

                Document document3 = new Document();
                document3.Add(new Field("Sentence", "关羽", Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(document3);

                Document document4 = new Document();
                document4.Add(new Field("Sentence", "两只小蜜蜂呀飞在花丛中呀", Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(document4);

                writer.Optimize();
            }

            //查找 这是一条测试的提交数据注释
            using (IndexSearcher searcher = new IndexSearcher(directory))
            {
                Term t = new Term("Sentence", "飞");//Sentence索引名称 飞 查询关键字
                Query query = new TermQuery(t);//构建一个查询为Term
                TopDocs docs = searcher.Search(query, null, 1000);//执行查询
                Console.WriteLine("符合条件的查询结果有:{0}", docs.TotalHits);//查询命中总数

                //循环可以被linq替代
                for (int i = 0; i < docs.TotalHits; i++)
                {
                    
                   Document doc = searcher.Doc(docs.ScoreDocs[i].Doc);//获取文档对象
                    Console.WriteLine(doc.Get("Sentence"));//读取文档对象中的内容字段
                }
            }
            Console.ReadKey();
        }
    }
}
