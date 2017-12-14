using System;
using System.Collections.Generic;

namespace ClusterinGAlgorithmForNetworks
{
    public class valueAndProb
    {
        double prob;
        int val;

        public valueAndProb()
        {
        }

        public valueAndProb(double p, int v)
        {
            prob = p;
            Value = v;
        }        

        public double Probability
        {
            get
            {
                return prob;
            }
            set
            {
                prob = value;
            }
        }

        public int Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }
    }
    public class Pair
    {
        Node first;
        Node second;

        public Pair()
        {
        }

        public Pair(Node f, Node s)
        {
            first = f;
            second = s;
        }

        public Node First
        {
            get
            {
                return first;
            }
            set
            {
                first = value;
            }
        }

        public Node Second
        {
            get
            {
                return second;
            }
            set
            {
                second = value;
            }
        }

        public static bool operator ==(Pair obj1, Pair obj2)
        {
            return (obj1.First == obj2.First && obj1.Second == obj2.Second);
        }

        public static bool operator !=(Pair obj1, Pair obj2)
        {
            return !(obj1.First == obj2.First && obj1.Second == obj2.Second);
        }

        public void print()
        {
            Console.Write("Pair is : " + first.Number + " " + second.Number);
        }
    }
    
    public class Node
    {
        int number;
        public List<valueAndProb> NodesAndProbability = new List<valueAndProb>();       

        public Node(int n)
        {
            number = n;
        }

        public Node()
        {
        }

        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        public bool isNeighbour(List <valueAndProb> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (Number == list[i].Value)
                    return true;
            }
            return false;
        }

        public void print()
        {
            Console.Write("Value is : ");
            Console.WriteLine(Number);
            
            for (int i = 0; i < NodesAndProbability.Count; i++)
            {
                Console.WriteLine("Probability is : " + NodesAndProbability[i].Probability);
                Console.WriteLine("Connected to : " + NodesAndProbability[i].Value + " node");
            }
        }
    }

    class Program
    {
        static public List<Pair> mentionedPairOfNodes = new List<Pair>();
        static Pair obj = new Pair(null,null);

        static public bool isMentioned(Pair obj)
        {
            for (int i = 0; i < mentionedPairOfNodes.Count; i++)
            {
                if (mentionedPairOfNodes[i] == obj)
                {
                    return true;
                }
            }
            return false;
        }

        static List<Node> CreatingNodes(int N)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 1; i <= N; i++)
            {
                Node n = new Node(i);
                nodes.Add(n);
            }
            return nodes;
        }
        static List<List<Node>> NodeToList(int N, int b)
        {
            List<List<Node>> nodeToList = new List<List<Node>>();
            for (int i = 0; i < N; i++)
            {
                nodeToList.Add(new List<Node>());
                nodeToList[i].Add(CreatingNodes(N)[i]);
            }
            return nodeToList;
        }
        static List<List<Node>> MakingClusters(int N, int b, List<List<Node>> a, double p)
        {
            List<List<Node>> groups = new List<List<Node>>();
            int numberOfNode = 0;
            Random random = new Random();
            double prob = 0.0;

            for (int i = 0; i < a.Count / b; i++)
            {
                groups.Add(new List<Node>());
                for (int j = 0; j < b; j++)
                {
                    for (int k = 0; k < N/ a.Count; k++)
                    {
                        groups[i].Add(a[numberOfNode][k]);
                    }
                    numberOfNode++;
                }
            }

            for (int j = 0; j < groups.Count; j++)
            {
                for (int k = 0; k < groups[j].Count - 1; k++)
                {

                    for (int h = k + 1; h < groups[j].Count; h++)
                    {

                        prob = random.NextDouble();
                        obj = new Pair( groups[j][k],groups[j][h]);
                        if (!isMentioned(obj))
                        {
                            if (prob >= 0 && prob <= p)
                            {
                                valueAndProb temp = new valueAndProb(prob, groups[j][h].Number);
                                groups[j][k].NodesAndProbability.Add(temp);
                            }
                            else
                            {
                                //valueAndProb temp = new valueAndProb(prob, 0);
                                //groups[j][k].NodesAndProbability.Add(temp);

                            }
                            mentionedPairOfNodes.Add(obj);
                        }
                        obj = new Pair(groups[j][h], groups[j][k]);
                        if (!isMentioned(obj))
                        {
                            if (prob >= 0 && prob <= p)
                            {
                                valueAndProb temp = new valueAndProb(prob, groups[j][k].Number);
                                groups[j][h].NodesAndProbability.Add(temp);
                            }
                            else
                            {
                                //valueAndProb temp = new valueAndProb(prob, 0);
                                //groups[j][h].NodesAndProbability.Add(temp);
                            }
                            mentionedPairOfNodes.Add(obj);
                        }
                    }
                }
            }
            return groups;          
        }

        static List<List<Node>> ClusteringAlgorithm(int N, int b, double p)
        {
            int i = 0;
            int Alpha = 1;
            p = Math.Pow(b, (-Alpha * (i + 1)));
            List<List<Node>> ListOfClusters = MakingClusters(N, b, NodeToList(N, b), p);         
           
            while (ListOfClusters.Count != 1)
            {
               
                Console.WriteLine("LEVEL {0} : ", i+1);
                Console.WriteLine("Clusters are: ");

                for (int k = 0; k < ListOfClusters.Count; k++)
                {
                    for (int j = 0; j < ListOfClusters[k].Count; j++)
                    {
                        Console.Write(ListOfClusters[k][j].Number + " ");
                    }
                    Console.WriteLine(" ");
                }
                Console.WriteLine("Probability range [ {0}, {1}]", 0, p);
                Print(N, b, ListOfClusters);
                i++;
                p = Math.Pow(b, (-Alpha * (i + 1)));
                ListOfClusters = MakingClusters(N, b, ListOfClusters, p);
                
            }
            p = Math.Pow(b, (-Alpha * (i+1)));
            Console.Write("LEVEL {0} : ", i + 1);
            Console.WriteLine("Clusters are: ");

            for (int k = 0; k < ListOfClusters.Count; k++)
            {
                for (int j = 0; j < ListOfClusters[k].Count; j++)
                {
                    Console.Write(ListOfClusters[k][j].Number + " ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine("Probability range [ {0}, {1}]", 0, p);
            Print(N, b, ListOfClusters);
            return ListOfClusters;
        }
    
        static void Print(int N, int b, List<List<Node>> list)
        {            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    list[i][j].print();
                }
                Console.WriteLine(" ");
            }           
        }

        static void Main(string[] args)
        {
            int N = 8;
            int b = 2;
            double p = 0;
            ClusteringAlgorithm(N, b, p);
        }
    }
}