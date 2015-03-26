using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoTSForETongModel;
using AutoTSForETongImplOfData;

namespace AutoTSForEtongCosole
{
    class Program
    {
        static SqlDbContext db = new SqlDbContext();
        static void Main(string[] args)
        {
            //将选择题中所有\n替换为<br/>
            IEnumerable<Question> questions = db.Questions.Where(o => o.QuestionType == "选择题");
            foreach (var item in questions)
            {
                item.Content = item.Content.Replace("\r\n","<br/>&nbsp;&nbsp;&nbsp;&nbsp;");
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
            questions = db.Questions.Where(o => o.QuestionType == "选择题");
            foreach (var item in questions)
            {
                Console.WriteLine(item.Content);
            }
        }
    }
}
