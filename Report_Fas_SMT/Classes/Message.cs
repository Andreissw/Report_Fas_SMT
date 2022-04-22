using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Report_Fas_SMT
{
    public class Message
    {
       
        public string Content { get; set; }      
        

        public  void RunEmail(string time)
        {                       
            var view = AlternateView.CreateAlternateViewFromString(Content, Encoding.UTF8, MediaTypeNames.Text.Html);  
         
            using (var client = new SmtpClient("mail.technopolis.gs", 25)) // Set properties as needed or use config file
            using (MailMessage message = new MailMessage()
            {
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                Subject = $"SMT-Report Карта контроля на {time}",
                SubjectEncoding = Encoding.UTF8,

            })

            {
                message.AlternateViews.Add(view);
                message.From = new MailAddress("reporter@dtvs.ru", "ROBOT");
                //message.From = new MailAddress("volodin1971@gmail.com", "Чувак");
                message.CC.Add("a.volodin@dtvs.ru");
                message.CC.Add("Овчинников Дмитрий Игоревич < ovchinnikov@dtvs.ru >");
                message.CC.Add("Парфенов Евгений Александрович <parfenov@dtvs.ru>");
                message.CC.Add("Мастер SMT <mastersmt@dtvs.ru>");
                message.CC.Add("Гусаров Валерий Вячеславович <gusarov@dtvs.ru>");
                message.CC.Add("Каспирович Дмитрий Иванович <kaspirovich@dtvs.ru>");
                message.CC.Add("Костина Ксения Викторовна <kostina@dtvs.ru>");          
                message.CC.Add("Лишик Станислав Александрович <lishik@dtvs.ru>");
                message.CC.Add("Мелехин Константин Данилович <melekhin@dtvs.ru>");
                message.CC.Add("Контролер ОТК <controlerotk@dtvs.ru>");
                message.CC.Add("Сузи Дмитрий Игоревич <d.suzi@dtvs.ru>");
                message.CC.Add("Фролов Дмитрий Андреевич <d.frolov@dtvs.ru>");
                message.CC.Add("Лихачёва Валерия Сергеевна <v.lihacheva@dtvs.ru>");
                message.CC.Add("Рыжков Иван Васильевич <i.ryjkov@dtvs.ru>");
                message.CC.Add("Салтыков Антон Андреевич <a.saltykov@dtvs.ru>");
                message.CC.Add("Ураев Александр Вячеславович <uraev@dtvs.ru>");
                message.CC.Add("Иванов Алексей Николаевич <a.n.ivanov@dtvs.ru>");
                message.CC.Add("Ескевич Дмитрий Сергеевич <d.eskevich@dtvs.ru>");
                message.CC.Add("Зайцев Григорий Владимирович <g.zaytsev@dtvs.ru>");
                message.CC.Add("Соловьев Никита Евгеньевич <n.e.solovyev@dtvs.ru>");
                message.CC.Add("Карнаухов Михаил Андреевич <m.karnauhov@dtvs.ru>");
                message.CC.Add("Андриевский Михаил Александрович <m.andrievskij@dtvs.ru>");
                message.CC.Add("Попов Денис Сергеевич <d.popov@dtvs.ru>");
                message.CC.Add("Шишкин Игорь Алексеевич <i.shishkin@dtvs.ru>");
                message.CC.Add("Баранова Мария Владимировна m.baranova@dtvs.ru");
                message.CC.Add("Юдин Денис Владимирович d.yudin@dtvs.ru");
                message.CC.Add("Силин Илья Дмитриевич i.silin@dtvs.ru");
                message.CC.Add("Зубец Виталий Анатольевич zubetc@dtvs.ru");
                message.CC.Add("Бондарай Александр Александрович <a.bondaray@dtvs.ru>");
                message.CC.Add("Чертаев Денис Александрович <d.chertaev@dtvs.ru>");
                message.CC.Add("Коротких Сергей Владимирович <korotkikh@dtvs.ru>");
                message.CC.Add("Гуторов Евгений Владимирович <gutorov@dtvs.ru>");

                client.Send(message);
              
            }      


        }

        public void RunEmailFAS(string time)
        {
            var view = AlternateView.CreateAlternateViewFromString(Content, Encoding.UTF8, MediaTypeNames.Text.Html);

            using (var client = new SmtpClient("mail.technopolis.gs", 25)) // Set properties as needed or use config file
            using (MailMessage message = new MailMessage()
            {
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                Subject = $"FAS-Report Карта контроля на {time}",
                SubjectEncoding = Encoding.UTF8,

            })

            {
                message.AlternateViews.Add(view);
                message.From = new MailAddress("reporter@dtvs.ru", "ROBOT");
                message.CC.Add("a.volodin@dtvs.ru");
                message.CC.Add("Ломакина Светлана Ивановна <s.lomakina@dtvs.ru>");
                message.CC.Add("Лишик Станислав Александрович <lishik@dtvs.ru>");
                message.CC.Add("Мелехин Константин Данилович <melekhin@dtvs.ru>");
                message.CC.Add("Костина Ксения Викторовна <kostina@dtvs.ru>");
                message.CC.Add("Климчук Андрей Михайлович <klimchuk@dtvs.ru>");
                message.CC.Add("Сидоров Владислав Леонидович <sidorov@dtvs.ru>");
                message.CC.Add("Ященко Петр Владимирович <yashenko@dtvs.ru>");
                message.CC.Add("Овчинников Дмитрий Игоревич < ovchinnikov@dtvs.ru >");
                message.CC.Add("Гусаров Валерий Вячеславович <gusarov@dtvs.ru>");
                message.CC.Add("Каспирович Дмитрий Иванович <kaspirovich@dtvs.ru>");
                message.CC.Add("Ткачук Алексей Владимирович <a.tkachuk@dtvs.ru>");
                message.CC.Add("Рыжков Иван Васильевич <i.ryjkov@dtvs.ru>");
                message.CC.Add("Лихачёва Валерия Сергеевна <v.lihacheva@dtvs.ru>");
                message.CC.Add("Контролер ОТК <controlerotk@dtvs.ru>");
                message.CC.Add("Салтыков Антон Андреевич <a.saltykov@dtvs.ru>");
                message.CC.Add("Ураев Александр Вячеславович <uraev@dtvs.ru>");
                message.CC.Add("Парфенов Евгений Александрович <parfenov@dtvs.ru>");
                message.CC.Add("Иванов Алексей Николаевич <a.n.ivanov@dtvs.ru>");
                message.CC.Add("Фоут Виктор Францевич <v.fout@dtvs.ru>");
                message.CC.Add("Мастер FAS <masterfas@dtvs.ru>");
                message.CC.Add("Ескевич Дмитрий Сергеевич <d.eskevich@dtvs.ru>");
                message.CC.Add("Зайцев Григорий Владимирович <g.zaytsev@dtvs.ru>");
                message.CC.Add("Соловьев Никита Евгеньевич <n.e.solovyev@dtvs.ru>");
                message.CC.Add("Карнаухов Михаил Андреевич <m.karnauhov@dtvs.ru>");
                message.CC.Add("Андриевский Михаил Александрович <m.andrievskij@dtvs.ru>");
                message.CC.Add("Попов Денис Сергеевич <d.popov@dtvs.ru>");
                message.CC.Add("Шишкин Игорь Алексеевич <i.shishkin@dtvs.ru>");
                message.CC.Add("Юдин Денис Владимирович d.yudin@dtvs.ru");
                message.CC.Add("Силин Илья Дмитриевич i.silin@dtvs.ru");
                message.CC.Add("Зубец Виталий Анатольевич zubetc@dtvs.ru");
                message.CC.Add("Баранова Мария Владимировна m.baranova@dtvs.ru");
                message.CC.Add("Бондарай Александр Александрович <a.bondaray@dtvs.ru>");
                message.CC.Add("Коваленко Игорь Владимирович <kovalenko@dtvs.ru>");
                message.CC.Add("Чертаев Денис Александрович <d.chertaev@dtvs.ru>");
                message.CC.Add("Коротких Сергей Владимирович <korotkikh@dtvs.ru>");
                message.CC.Add("Гуторов Евгений Владимирович <gutorov@dtvs.ru>");

                client.Send(message);

            }


        }
    }
}
