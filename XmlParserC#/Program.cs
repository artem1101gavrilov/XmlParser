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
        public enum Status { NOT_RECEIVED, ACTIVE, DONE };

        public Status status;
        public int id; //Номер квеста
        public string name; //Текстовое поле для кнопки
        public string title; // Оглавление Квеста
        public string description; // Описание квеста
        public string toDo; // что надо сделать в квесте
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
                    else if (itemItens.Name == "status") newQuest.status = (Quest.Status)int.Parse(itemItens.InnerText);
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
            
            Console.Write("XML parser\n");

            XmlDocument document = new XmlDocument();
            document.Load("QuestData2.xml");

            XmlNodeList dataList = document.GetElementsByTagName("quest");

            foreach (XmlNode item in dataList)
            {
                Quest newQuest = new Quest();
                newQuest.id = int.Parse(item.Attributes[0].InnerText); // TODO to int
                newQuest.status = (Quest.Status)int.Parse(item.Attributes[1].InnerText);
                newQuest.name = item.Attributes[2].InnerText;
                newQuest.title = item.Attributes[3].InnerText;
                newQuest.description = item.Attributes[4].InnerText;
                newQuest.toDo = item.Attributes[5].InnerText;
                QuestList.Add(newQuest);
            }

            foreach (Quest value in QuestList)
            {
                Console.Write(value.id + " " + value.name + "\n");
            }
        }

        //По считанному XML создадим новый:
        //Увеличим для примера какой-нибудь параметр
        //Сохраним файл
        //Удалим старый
        //Переименуем
        public static void CreateXMLQuest()
        {
            XmlTextWriter writer = new XmlTextWriter("QuestData3.xml", Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("quests");
            //тут можно или даже нужно отсортировать данные по полю id и соотвественно поменять цикл или сделать сортировку до этого.
            foreach (Quest quest in QuestList)
            {
                writer.WriteStartElement("quest");
                writer.WriteAttributeString("id", (quest.id + 1).ToString());
                writer.WriteAttributeString("status", ((int)(quest.status)).ToString());
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
        }

        static void Main(string[] args)
        {
            QuestFunc2();
            CreateXMLQuest();

            Console.ReadKey(); 
        }
    }
}
