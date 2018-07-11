using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace CsharpXml
{
    public class Quest
    {
        public DateTime data;
        public int status;
        public int id; //Номер квеста
        public string name; //Текстовое поле для кнопки
        public string title; // Оглавление Квеста
        public string description; // Описание квеста
        public string toDo; // что надо сделать в квесте

        public void SetParam(Quest QuestParam)
        {
            data = QuestParam.data;
            status = QuestParam.status;
            id = QuestParam.id;
            name = QuestParam.name;
            title = QuestParam.title;
            description = QuestParam.description;
            toDo = QuestParam.toDo;
        }
    }

    class Program
    {
        public static void QuestFunc1()
        {
            //Для xml файлов где структура имеет следующий вид:
            /*
              <quest>
                    <id>1</id>
                    <status>0</status>
                    <name>Квест 1</name>
                    <title>ЛАЛАЛА</title>
                    <description>ЛАЛАЛА</description>
                    <toDo>ЛАЛАЛА</toDo>
              </quest>
             */
            List<Quest> QuestList = new List<Quest>(); //Лист со всеми квестами
            Console.Write("XML parser\n");

            XmlDocument document = new XmlDocument();
            document.Load("QuestData.xml");

            XmlNodeList dataList = document.GetElementsByTagName("quest");

            foreach (XmlNode item in dataList)
            {
                XmlNodeList itemContent = item.ChildNodes;
                Quest newQuest = new Quest();
                foreach (XmlNode itemItens in itemContent)
                {
                    if (itemItens.Name == "id") newQuest.id = int.Parse(itemItens.InnerText); // TODO to int
                    else if (itemItens.Name == "status") newQuest.status = int.Parse(itemItens.InnerText);
                    else if (itemItens.Name == "name") newQuest.name = itemItens.InnerText;
                    else if (itemItens.Name == "title") newQuest.title = itemItens.InnerText;
                    else if (itemItens.Name == "description") newQuest.description = itemItens.InnerText;
                    else if (itemItens.Name == "toDo") newQuest.toDo = itemItens.InnerText;
                }
                QuestList.Add(newQuest);
            }

            foreach (Quest value in QuestList)
            {
                Console.Write(value.id + " " + value.name + "\n");
            }
        }

        public static List<Quest> QuestList = new List<Quest>(); //Лист со всеми квестами

        public static void QuestFunc2()
        {
            //Для xml файлов где структура имеет следующий вид:
            /*
              <quest id = "1" status = "0" name = "Квест 1" title = "ЛАЛАЛА" description = "ЛАЛАЛА" toDo = "ЛАЛАЛА"/>
            */
            
            Console.Write("XML parser start\n");

            XmlDocument document = new XmlDocument();
            document.Load("QuestData2.xml");

            XmlNodeList dataList = document.GetElementsByTagName("quest");

            foreach (XmlNode item in dataList)
            {
                Quest newQuest = new Quest();
                newQuest.data = DateTime.Parse(item.Attributes[0].InnerText);
                newQuest.id = int.Parse(item.Attributes[1].InnerText); // TODO to int
                newQuest.status = int.Parse(item.Attributes[2].InnerText);
                newQuest.name = item.Attributes[3].InnerText;
                newQuest.title = item.Attributes[4].InnerText;
                newQuest.description = item.Attributes[5].InnerText;
                newQuest.toDo = item.Attributes[6].InnerText;
                QuestList.Add(newQuest);
            }

            /*foreach (Quest value in QuestList)
            {
                Console.Write(value.id + " " + value.name + "\n");
            }*/

            Console.Write("XML parser finish\n");
        }

        //По считанному XML создадим новый:
        //Увеличим для примера какой-нибудь параметр
        //Сохраним файл
        //Удалим старый
        //Переименуем
        public static void CreateXMLQuest()
        {
            //Сортировка нашего листа по ключевому полю id
            //Сортировка пузырьком
            for (int i = 0; i < QuestList.Count - 1; i++)
            {
                for (int j = 0; j < QuestList.Count - i - 1; j++)
                {
                    if (QuestList[j].data > QuestList[j + 1].data)
                    {
                        Quest buf = new Quest();
                        buf.SetParam(QuestList[j]);
                        QuestList[j].SetParam(QuestList[j + 1]);
                        QuestList[j + 1].SetParam(buf);
                    }
                }
            }


            Console.Write("XML create start\n");
            XmlTextWriter writer = new XmlTextWriter("QuestData3.xml", Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("quests");
            foreach (Quest quest in QuestList)
            {
                writer.WriteStartElement("quest");
                writer.WriteAttributeString("data", quest.data.ToShortDateString());
                writer.WriteAttributeString("id", (quest.id).ToString());
                writer.WriteAttributeString("status", (quest.status).ToString());
                writer.WriteAttributeString("name", quest.name);
                writer.WriteAttributeString("title", quest.title);
                writer.WriteAttributeString("description", quest.description);
                writer.WriteAttributeString("toDo", quest.toDo);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();

            File.Delete("QuestData2.xml");
            File.Move("QuestData3.xml", "QuestData2.xml");
            Console.Write("XML create finish\n");
        }

        static void Main(string[] args)
        {
            QuestFunc2();
            CreateXMLQuest();

            /*
            DateTime date1 = new DateTime();
            date1 = DateTime.Today;
            Console.WriteLine(date1.ToShortDateString());*/

            Console.ReadKey(); 
        }
    }
}
