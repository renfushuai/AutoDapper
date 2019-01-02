using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Newtonsoft.Json;
using XDF.Core.Validation;

namespace XDF.Test
{
    public class StudentInfo : Person
    {
        public StudentInfo()
        {
            Name = "student";
            Age = 18;
        }
        [JsonIgnore]
        public DateTime Birthday { get; set; }
    }

    public class Teacher : Person
    {
        public Teacher()
        {
            Name = "teacher";
            Age = 10;
        }

    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        [Price("价格不正确")]
        public decimal Price { get; set; }

    }
    public class UnitTest1
    {


        [Fact]
        public void Test2()
        {
            Person p = new Person();
            p.Price = 10.0M;
            var res = p.Validation<Person, Person>();
        }


        [Fact]
        public void Test3()
        {
            Format.Formater();
        }
        public class Block
        {
            public List<string> Contents { get; set; } = new List<string>();
            public int Count { get; set; } = 0;
        }

        public class Format
        {
            public static void Formater()
            {
                var str = @"id,nSchoolId,sBatchCode,dShouldReceive,dRealReceive,dtSbmmitDate,sOpportunityOwner,nBatchStatus,nChannel,sQuote,sOrderCode,sCreatedOperator,bDisposed,sOperateTypeCode,sChangedItemCode,sBatchGuid,nBatchSerial,dtModify,sBusinessCode,nSubChannel,sSystemSource,sMarketingSources,sMarketingSourcesExt,nPayStatus,sPayOrderCode,sRecalOrder,nAllotStatus ";
                var list = str.Split(",").Select(s => s.Trim()).OrderBy(s => s.Length);
                var blocks = new List<Block>();
                var list1 = list.Where(s => s.Length < 18).ToList();
                var list2 = list.Where(s => s.Length < 38 && s.Length >= 18).ToList();
                var list3 = list.Where(s => s.Length < 58 && s.Length >= 38).ToList();
                var list4 = list.Where(s => s.Length < 78 && s.Length >= 58).ToList();
                var list5 = list.Where(s => s.Length < 98 && s.Length >= 78).ToList();
                blocks.AddRange(list5.Select(b => new Block
                {
                    Contents = new List<string> { b}
                }));
                foreach (var item in list4)
                {
                    var block = new Block();
                    block.Contents.Add(item);
                    if (list1.Any())
                    {
                        block.Contents.Add(list1.ElementAt(0));
                        list1.RemoveAt(0);
                    }
                    blocks.Add(block);
                }
                foreach (var item in list3)
                {
                    var block = new Block();
                    block.Count = 3;
                    block.Contents.Add(item);
                    if (list2.Any())
                    {
                        block.Contents.Add(list.ElementAt(0));
                        continue;
                    }
                    while (list1.Any() && block.Count < 5)
                    {
                        block.Contents.Add(list1.ElementAt(0));
                        list1.RemoveAt(0);
                        block.Count += 1;
                    }
                    blocks.Add(block);
                }
                var block2 = new Block();
                foreach (var item in list2)
                {
                    if ((block2.Count == 4 && !list1.Any()) || block2.Count == 5)
                    {
                        blocks.Add(new Block { Contents = block2.Contents });
                        block2 = new Block();
                    }
                    block2.Contents.Add(item);
                    block2.Count += 2;
                    if (block2.Count == 4 && list1.Any())
                    {
                        block2.Contents.Add(list1.ElementAt(0));
                        block2.Count += 1;
                        list1.RemoveAt(0);
                    }
                }
                foreach (var item in list1)
                {
                    if (block2.Count == 5)
                    {
                        blocks.Add(new Block { Contents = block2.Contents });
                        block2 = new Block();
                    }
                    block2.Contents.Add(item);
                    block2.Count += 1;
                }
                var i = 0;
                foreach (var block in blocks)
                {
                    foreach (var item in block.Contents)
                    {
                        if (item.Length >= 78)
                        {
                            Console.Write($"{item},".PadRight(95));
                        }
                        else if (item.Length >= 58)
                        {
                            Console.Write($"{item},".PadRight(76));
                        }
                        else if (item.Length >= 38)
                        {
                            Console.Write($"{item},".PadRight(57));
                        }
                        else if (item.Length >= 18)
                        {
                            Console.Write($"{item},".PadRight(38));
                        }
                        else
                        {
                            Console.Write($"{item},".PadRight(19));
                        }
                    }
                    Console.Write("\r\n");
                }
            }
        }

    }
}
