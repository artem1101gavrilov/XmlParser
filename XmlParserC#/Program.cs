using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

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
        
        static void Main(string[] args)
        {
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
            Console.ReadKey(); 
        }
    }
}
