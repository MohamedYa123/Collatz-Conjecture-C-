    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace CollatzConjectureProver
    {
        public struct sumQandSumZ
        {
            public int SumQ ;
            public int SumZ;
        }
        public class CollatzCalculator
        {
            public int GlobeSumQ = 0;
            public int GlobeSumZ = 0;
            public const float log2_3Over2 =  0.58496250072117f;//496250072116f;//0.58496250072116//2.3219f;1.807354922f;//
            public double PercentageProved=0;
            float GfromQ;
            float GtoQ;
            float Qpercentage;
            float Zpercentage;
            public int mode = 0;
            public float Percentage
            {
                get
                {
                    if (mode == 1)
                {
                    return 1;
                }
                    var a = MathF.Pow(0.5f, GfromQ);
                    var n = GtoQ - GfromQ;
                    var nz = GtoQ - 1;
                    Qpercentage= a * (1 - MathF.Pow(0.5f, n)) / (1 - 0.5f);
                    Zpercentage = 1;// 0.5f * (1 - MathF.Pow(0.5f, nz)) / (1 - 0.5f);
                    return Qpercentage*Zpercentage;
                }
            }
            public  double RelativePercentage { get
                {
                if (mode == 1)
                {
                    return 1;
                }
                    return PercentageProved*Percentage;
                } }
            //
            public int timingFactor =3;
            public int addingFactor = 1;
            //= > timingFactor*Oddnum+addingFactor
            public int dividingFactor=2;
            //= > Evennum/dividingFactor
            //Search never ends
            public void Searchnum(int Q,int B,double basicPercentage)
            {

            }
            public void calcallnumbers(int depth,int barrier=0,double perc=1)
            {
            if (perc == 1)
            {
                sp=Stopwatch.StartNew();
            }
                if (barrier < 0)
                {
                    PercentageProved += perc;
                    return;
                }
                if (depth == 0)
                {
                    return;
                }
                int barrierplus = barrier+1;
                int barrierminus = barrier-1;
                for(int prop=1;prop<=10;prop++)
                {
                    if (prop <= 7)
                    {
                    calcallnumbers(depth - 1, barrierminus, perc * 0.1);
                    }
                    else
                    {
                    calcallnumbers(depth - 1, barrierplus, perc * 0.1);
                    }
                }
            if (perc == 1)
            {
                sp.Stop();
            }
        }
            bool First=true;
            int FirstDepth=0;
            List<float> valuestimed = new List<float>();
            float[] valuestimed2 = new float[10000];
            public Stopwatch sp=new Stopwatch();
            public int highest;
            List<functionParamaters> functionParamaters = new List<functionParamaters>();
            public void SearchByQ_BYOneShot(int Qmain, int fromQ, int toQ, int Depth, float MiniPercentage = 1, int SumQ = 0, int SumZ = 0, bool StoreOnly = false)
            {

                float QuickPow(float value, int pow)
                {
                    float ans = 1f;
                    for (int i = 0; i < pow; i++)
                    {
                        ans *= value;
                    }
                    return ans;
                }

                float quickpow_half(int pow)
                {
                    //return QuickPow(0.5f,pow);
                    if (pow > highest)
                    {
                        highest = pow;
                    }
                    if (valuestimed2[pow] != 0)
                    {
                        return valuestimed2[pow];
                    }
                    else
                    {
                        valuestimed2[pow] = QuickPow(0.5f, pow); ;
                        return valuestimed2[pow];
                    }
                }
                float quickpow_half2(int pow)
                {
                    if (pow > highest)
                    {
                        highest= pow;
                    }
                    switch (pow < valuestimed.Count)
                    {
                        case true:
                            return valuestimed[pow];
                        case false:
                            int pow2 = valuestimed.Count;
                            while (true)
                            {
                                float val = QuickPow(0.5f, pow2);
                                valuestimed.Add(val);
                                if (pow == valuestimed.Count - 1)
                                {
                                    break;
                                }
                                pow2++;
                            }
                            return valuestimed[pow];
                    }
                }
                float quickmutliply(int first,float second)
                {
                    float ans = second;
                    for(int i=1;i<first; i++)
                    {
                        ans += second;
                    }
                    return ans;
                }
                if (First)
                {
                    GfromQ = fromQ;
                    GtoQ=toQ;
                    FirstDepth = Depth;
                    First = false;
                    SumQ = Qmain;
                    SumZ = 0;
                    PercentageProved = Percentage * 0;
                    sp.Restart();
                }
                int maxz = (int) (SumQ * log2_3Over2)+1;//(quickmutliply(SumQ,log2_3Over2))+1;//
                if (SumZ >= maxz)
                {
                    PercentageProved += MiniPercentage;
                    return;
                }
                maxz -= SumZ;
                maxz = Math.Min(maxz, 7);
                PercentageProved += MiniPercentage * quickpow_half(maxz - 1);///percentage;
                if (Depth==0|| PercentageProved == 1)
                {
                    if (PercentageProved==1)
                    {
                        sp.Stop();
                    }
                    return;
                }
                //
                var functionparamater = new functionParamaters { Qmain = 1, fromQ = fromQ, toQ = toQ, MiniPercentage = MiniPercentage, SumQ = SumQ, SumZ = SumZ,maxz=maxz };
                if (StoreOnly)
                {
                    functionParamaters.Add(functionparamater);
                    return;
                }
                startloop(functionparamater, Depth,StoreOnly);
                //
                //for (int z = 1; z < maxz; z++)
                //{
                //    float minipercentageZ = quickpow_half(z);
                //    for (int Q = fromQ;  Q<= toQ; Q++)
                //    {
                //        float minipercentageQ = quickpow_half(Q);
                //        switch (StoreOnly)
                //        {
                //            case true:

                //                //functionParamaters.Push(new functionParamaters{ Qmain = Q, fromQ=fromQ,toQ=toQ,MiniPercentage= MiniPercentage * minipercentageQ * minipercentageZ,SumQ=SumQ+Q,SumZ=SumZ+z });
                //                functionParamaters.Add(new functionParamaters { Qmain = Q, fromQ = fromQ, toQ = toQ, MiniPercentage = MiniPercentage * minipercentageQ * minipercentageZ, SumQ = SumQ + Q, SumZ = SumZ + z });
                //                //functionParamaters.Push(new functionParamaters { Qmain = Q, fromQ = fromQ, toQ = toQ, MiniPercentage = MiniPercentage * minipercentageQ * minipercentageZ, SumQ = SumQ + Q, SumZ = SumZ + z });
                //                break;
                //            case false:
                //               SearchByQ_BYOneShot(Q, fromQ, toQ, Depth - 1, MiniPercentage: MiniPercentage * minipercentageQ * minipercentageZ, SumQ: SumQ+Q, SumZ: SumZ + z);
                //                break;
                //        }
                //    }
                //}
                if (Depth == FirstDepth&&!StoreOnly)
                {
                    sp.Stop();
                }
            
            }
            void startloop(functionParamaters functionParamater,int Depth,bool storeonly)
            {
                float QuickPow(float value, int pow)
                {
                    float ans = 1f;
                    for (int i = 0; i < pow; i++)
                    {
                        ans *= value;
                    }
                    return ans;
                }

                float quickpow_half(int pow)
                {
                    //return QuickPow(0.5f,pow);
                    if (pow > highest)
                    {
                        highest = pow;
                    }
                    if (valuestimed2[pow] != 0)
                    {
                        return valuestimed2[pow];
                    }
                    else
                    {
                        valuestimed2[pow] = QuickPow(0.5f, pow); ;
                        return valuestimed2[pow];
                    }
                }
                var maxz = functionParamater.maxz;
                var toQ = functionParamater.toQ;
                var fromQ = functionParamater.fromQ;
                var SumQ = functionParamater.SumQ;
                var SumZ=functionParamater.SumZ;
                var MiniPercentage=functionParamater.MiniPercentage;
                var zs = Math.Min(maxz, toQ+1);
                zs = maxz;
                for (int z = 1; z < zs; z++)
                {
                    float minipercentageZ = quickpow_half(z)/Zpercentage;
                    for (int Q = fromQ; Q <= toQ; Q++)
                    {
                        float minipercentageQ = quickpow_half(Q)/Qpercentage ;
                    
                        SearchByQ_BYOneShot(Q, fromQ, toQ, Depth - 1, MiniPercentage: MiniPercentage * minipercentageQ * minipercentageZ, SumQ: SumQ + Q, SumZ: SumZ + z,StoreOnly:storeonly);
                           
                    }
                }
            }
            public int DepthReached;
            public void SearchByQ_Parallel(int Qmain,int fromQ,int toQ, int Depth, int SumQ = 0, int SumZ = 0)
            {
                sp.Restart();
                SearchByQ_BYOneShot(Qmain, fromQ, toQ, Depth, StoreOnly: true);
                for (int depth=2; depth<=Depth+1; depth++)
                {
                    var oldstack = functionParamaters;
                    functionParamaters = new List<functionParamaters>();
                    int c = 0;
                    foreach(var functionParamatersStruct in oldstack) //(oldstack.Count>c)
                    {
                        //var functionParamatersStruct=oldstack[c];
                        //SearchByQ_BYOneShot(functionParamatersStruct.Qmain, functionParamatersStruct.fromQ, functionParamatersStruct.toQ, depth, functionParamatersStruct.MiniPercentage, functionParamatersStruct.SumQ, functionParamatersStruct.SumZ, StoreOnly: true);
                        startloop(functionParamatersStruct, depth,true);
                        c++;
                    }
                    //oldstack.Clear();
                    oldstack = null;
                    //GC.Collect();
                    DepthReached = depth;

                }
                sp.Stop();
            }
        }
        public struct functionParamaters
        {
           public int Qmain;
           public int fromQ;
           public int toQ; 
           public float MiniPercentage;
           public int SumQ;
           public int SumZ;
            public int maxz;
        }
        public class Tree
        {

        }
        public class Block
        {

        }
    }
