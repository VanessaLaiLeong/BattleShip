using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.SymbolStore;

namespace BatalhaNaval
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            //inicio da partida
            int opcao = 0, opcao2 =0;
            int dim = 10;
            bool PVPc = false;
            bool jogo = true;
            int shipsJ1 = 0;
            int shipsJ2 = 0;
            int jogadorAtual = 1;         
            string nome = "";

            

            List<Jogador> lst_vencedores = new List<Jogador>();

            

            do
            {
                menu();
                opcao = int.Parse(Console.ReadLine());

                Jogador j1;
                Jogador j2;

                j1 = new Jogador();
                j2 = new Jogador();

                Console.Clear();

                switch (opcao)
                {


                    case 1://PvP 

                        do
                        {
                            menu2();
                            opcao2 = int.Parse(Console.ReadLine());

                            switch (opcao2)
                            {
                                case 1://nivel facil

                                    runGame(j1, j2, nome, 10, jogo, jogadorAtual, shipsJ1, shipsJ2, lst_vencedores, false);

                                    printScores("Vencedores", lst_vencedores);

                                    break;

                                case 2: //nivel medio

                                    runGame(j1, j2, nome, 15, jogo, jogadorAtual, shipsJ1, shipsJ2, lst_vencedores, false);

                                    printScores("Vencedores", lst_vencedores);

                                    break;

                                case 3: //nivel dificil

                                    runGame(j1, j2, nome, 20, jogo, jogadorAtual, shipsJ1, shipsJ2, lst_vencedores, false);

                                    printScores("Vencedores", lst_vencedores);


                                    break;

                                default : 
                                    opcao = 4;
                                    break;

                                    
                            }
                            


                                

                        }
                        while (opcao != 4);
                        
                       

                        break;

                    case 2://PVPc

                        do
                        {
                            menu2();
                            opcao2 = int.Parse(Console.ReadLine());

                            switch (opcao2)
                            {
                                case 1:

                                    runGame(j1, j2, nome, 10, jogo, jogadorAtual, shipsJ1, shipsJ2, lst_vencedores, true);

                                    printScores("Vencedores", lst_vencedores);

                                    break;

                                case 2:

                                    runGame(j1, j2, nome, 15, jogo, jogadorAtual, shipsJ1, shipsJ2, lst_vencedores, true);

                                    printScores("Vencedores", lst_vencedores);

                                    break;

                                case 3:

                                    runGame(j1, j2, nome, 20, jogo, jogadorAtual, shipsJ1, shipsJ2, lst_vencedores, true);

                                    printScores("Vencedores", lst_vencedores);

                                    break;

                                default:
                                    opcao = 4;
                                    break;
                            }



                        }
                        while (opcao != 4);


                        
                        break;

                      

                }

                    

            }

            while (opcao != 3);
















            Console.ReadKey();




        }

        //funcao principal do jogo
        static void runGame (Jogador j1, Jogador j2, string nome, int dim , bool jogo, int jogadorAtual, 
            int shipsJ1, int shipsJ2, List<Jogador> lst_vencedores, bool PVPc)
        {

            

            //Jogador 1
            nome = setNome();
            j1.nome = nome;
            j1.tabuleiroPecas = gerarTabuleiro(dim);
            j1.tabuleiroJogadas = gerarTabuleiro(dim);
            j1.numeroJogadas = 0;
            Console.Clear();
            setPecas(j1, false);



            //Jogador 2
            nome = setNome();
            j2.nome = nome;
            j2.tabuleiroPecas = gerarTabuleiro(dim);
            j2.tabuleiroJogadas = gerarTabuleiro(dim);
            j2.numeroJogadas = 0;
            Console.Clear();
            setPecas(j2, PVPc);




            
            while (jogo == true)
            {
                if (jogadorAtual == 1)
                {
                    jogar(j1, j2, false);
                    Console.Clear();
                    j1.numeroJogadas++;
                    jogadorAtual++;

                }
                else
                {
                    jogar(j2, j1, PVPc);
                    Console.Clear();
                    j2.numeroJogadas++;
                    jogadorAtual--;

                }

                //contagem do nr de jogadas para o player 1
                for (int i = 0; i < j1.tabuleiroPecas.GetLength(0); i++)
                {
                    for (int j = 0; j < j1.tabuleiroPecas.GetLength(1); j++)
                    {
                        if (j1.tabuleiroPecas[i, j] == 'S')
                            shipsJ1++;

                    }

                }

                //contagem do nr de jogadas para o player 2
                for (int i = 0; i < j2.tabuleiroPecas.GetLength(0); i++)
                {
                    for (int j = 0; j < j2.tabuleiroPecas.GetLength(1); j++)
                    {
                        if (j2.tabuleiroPecas[i, j] == 'S')
                            shipsJ2++;

                    }

                }

                //condicao para vencer - quando os navios do adversário chega a zero
                if (shipsJ1 == 0)
                {
                    Console.WriteLine($"{j2.nome} venceu!");
                    jogo = false; //jogo termina
                    lst_vencedores.Add(j2);
                }

                if (shipsJ2 == 0)
                {
                    Console.WriteLine($"{j1.nome} venceu!");
                    jogo = false;
                    lst_vencedores.Add(j1);
                }

                shipsJ1 = 0;
                shipsJ2 = 0;
            }

            printTabuleiro(j1.tabuleiroPecas);
            printTabuleiro(j2.tabuleiroPecas);
        }


        
       

        //antes do jogar
        static void setPecas(Jogador jogador, bool PVPc)
        {
            string[] navios = { "Submarino (1)", "Submarino (1)", "Corveta (2)", "Corveta (2)", "Fragata (3)", "Fragata (3)", "Porta-aviões (4)", "Porta-aviões (4)" };

            int[] dimNavios = {1,1,2,2,3,3,4,4};

            
            string orientacao = "";

            for (int i = 0; i < dimNavios.Length; i++)
            {
                Coordenada coordenadas = new Coordenada();
                bool outOfBounds;
                bool posicaoValida;

                do //verificaçao se a casa está livre ou seja sem peças
                {
                    outOfBounds = false;
                    posicaoValida = true;
                    Console.WriteLine($"{jogador.nome}"); // aparece o nome do jogador
                    if (PVPc == false)
                    {
                        printTabuleiro(jogador.tabuleiroPecas);
                    }

                    //modo PVP
                    if (PVPc == false)
                    {
                        do //orientacao da peca
                        {
                            Console.WriteLine($"Indique a orientacao (V/H) da peça {navios[i]}");
                            orientacao = Console.ReadLine().ToLower();
                        }
                        while (orientacao != "v" && orientacao != "h");
                    }

                    //modo contra PC
                    else
                    {
                        Random rnd = new Random();
                        int val = rnd.Next(0,2);
                        if(val == 0)
                        {
                            orientacao = "v";
                        }
                        else
                        {
                            orientacao = "h";
                        }
                    }


                    //validar coordenadas
                    getCoordenadas(coordenadas, PVPc, jogador.tabuleiroJogadas.GetLength(0));



                   

                    for (int j = 0; j < dimNavios[i]; j++)
                    {
                        if (orientacao == "v") //vertical
                        {   

                            //excecao para se for fora do tabuleiro
                            if(dimNavios[i]-1 + coordenadas.linha - 1 >= jogador.tabuleiroJogadas.GetLength(0))
                            {
                                outOfBounds = true;
                                break;
                            }

                            // verificar se tem barcos nas posicoes das pecas
                            if (jogador.tabuleiroPecas[coordenadas.linha - 1 + j, coordenadas.coluna - 1] == 'S')
                            {
                                Console.WriteLine("!!!!!Ja existe um barco nesta posicao!!!!!"); // execoes para se ja houver peca no tabuleiro nessa posicao
                                if (PVPc == false)
                                {
                                    Console.ReadLine();
                                }
                                
                                Console.Clear();
                                posicaoValida = false;
                                break;
                            }
                        }
                        else //horizontal
                        {
                            if (dimNavios[i]-1 + coordenadas.coluna - 1 >= jogador.tabuleiroJogadas.GetLength(0))
                            {
                                outOfBounds = true;
                                break;
                            }
                            if (jogador.tabuleiroPecas[coordenadas.linha - 1, coordenadas.coluna - 1 +j] == 'S')
                            {
                                Console.WriteLine("!!!!!Ja existe um barco nesta posicao!!!!!"); // execoes para se ja houver peca no tabuleiro nessa posicao
                                if (PVPc == false)
                                {
                                    Console.ReadLine();
                                }
                                Console.Clear();
                                posicaoValida = false;
                                break;
                            }
                        }

                    }
                    if (posicaoValida==true) 
                    { 
                        for (int j = 0; j < dimNavios[i]; j++)
                        {
                            if (orientacao == "v")
                            {
                                if (outOfBounds)
                                {
                                    break;
                                }
                                jogador.tabuleiroPecas[coordenadas.linha - 1 + j, coordenadas.coluna - 1] = 'S'; // no local da escolha fica um S
                            }
                            else
                            {
                                if (outOfBounds)
                                {
                                    break;
                                }
                                jogador.tabuleiroPecas[coordenadas.linha - 1, coordenadas.coluna - 1 +j] = 'S';
                            }
                        }
                    }
                }
                while (posicaoValida==false || outOfBounds);

                

                
                if (PVPc == false)
                {
                    printTabuleiro(jogador.tabuleiroPecas);
                }
                

                Console.Clear();

            }
            if (PVPc == false)
            {
                printTabuleiro(jogador.tabuleiroPecas);
            }
            Console.Clear();

        }


        
        static Coordenada getCoordenadas(Coordenada coord, bool PVPc, int dim)
        {
            do
            {
                
                // vs Pc
                if (PVPc == true)
                {
                    Random rnd = new Random();
                    coord.linha = rnd.Next(1, dim+1);
                    coord.coluna = rnd.Next(1, dim+1);
                }
                else
                {
                    //excessoes da linha de letras ou null
                    Console.WriteLine($"Insira a primeira coordenada (Linha)");
                    try
                    {
                        coord.linha = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input inválido. Por favor insira somente números.");
                        Console.ReadLine();

                        continue;
                    }
                    Console.WriteLine($"Insira a segunda coordenada (Coluna)");

                    //excessoes da coluna de letras ou null
                    try
                    {
                        coord.coluna = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input inválido. Por favor insira somente números.");
                        continue;
                    }


                    if (coord.linha < 1 || coord.linha > dim || coord.coluna < 1 || coord.coluna > dim ) // execoes para coordenadas invalidas
                    {
                        Console.WriteLine("!!!!!Coordenadas invalidas!!!!!!");
                        Console.ReadLine();

                    }

                }
                
            }
            while (coord.linha < 1 || coord.linha > dim || coord.coluna < 1 || coord.coluna > dim);

            return coord;

        }

        
        static void jogar(Jogador jogadorAtual, Jogador jogadorEspera, bool PVPc) 
        {
            
            Coordenada coordenada = new Coordenada();
            

            Coordenada coordenada12 = new Coordenada();
            

            Console.WriteLine($"Vez do: {jogadorAtual.nome}\n");
                      
            Console.WriteLine("Tabuleiro das Peças");
            if (PVPc == false)
            {
                printTabuleiro(jogadorAtual.tabuleiroPecas);
            }

            Console.WriteLine("Tabuleiro das Jogadas");
            if (PVPc == false)
            {
                printTabuleiro(jogadorAtual.tabuleiroJogadas);
            }


            //validar coordenadas
            do
            {
                getCoordenadas(coordenada, PVPc, jogadorAtual.tabuleiroJogadas.GetLength(0));
            }
            while (jogadorEspera.tabuleiroPecas[coordenada.linha - 1, coordenada.coluna - 1] == 'A' || jogadorEspera.tabuleiroPecas[coordenada.linha - 1, coordenada.coluna - 1] == 'X');
            


            Console.Clear();
            
                if (jogadorEspera.tabuleiroPecas[coordenada.linha - 1, coordenada.coluna - 1] == 'S')
                {

                    Console.WriteLine("!!!HIT!!!!\n");
                    jogadorAtual.tabuleiroJogadas[coordenada.linha - 1, coordenada.coluna - 1] = 'X';
                    jogadorEspera.tabuleiroPecas[coordenada.linha - 1, coordenada.coluna - 1] = 'X';

                }
                else
                {

                    Console.WriteLine("!!!AGUA!!!\n");
                    jogadorAtual.tabuleiroJogadas[coordenada.linha - 1, coordenada.coluna - 1] = 'A';

                    jogadorEspera.tabuleiroPecas[coordenada.linha - 1, coordenada.coluna - 1] = 'A';

                }
            
            
            Console.WriteLine("Enter para continuar");
            Console.ReadLine();





        }        


        

        

        //tabuleiros

        static char [,] gerarTabuleiro(int dim)
        {
            char[,] tabuleiro = new char[dim, dim];

            for (int i = 0; i < tabuleiro.GetLength(0); i++)
            {
                for (int j = 0; j < tabuleiro.GetLength(1); j++)
                {
                    tabuleiro[i, j] = '-';
                    
                }
                
            }
            return tabuleiro;

        }


        static void printTabuleiro(char [,] tabuleiro)

        {
            //print dos numeros que indicam coluna
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("   ");
            for (int i = 1; i <= tabuleiro.GetLength(0); i++)
            {
                Console.Write("{0,2} ", i); //da 2 espacos para cada i
            }
            Console.WriteLine();
            Console.ResetColor();

            for (int i = 0; i < tabuleiro.GetLength(0); i++)
            {
                //print dos numeros que indicam as linhas
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0,3} ", i + 1);
                Console.ResetColor();
                for (int j = 0; j < tabuleiro.GetLength(1); j++)
                {
                    if (tabuleiro[i, j] == 'S')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        

                    }
                    if (tabuleiro[i, j] == 'X')
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        

                    }
                    if (tabuleiro[i, j] == '-')
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        

                    }
                    if (tabuleiro[i, j] == 'A')
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;


                    }
                    Console.Write("{0,2} ", tabuleiro[i, j]);

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }



        //menus
        static void menu()
        {
            Console.WriteLine("JOGO DA BATALHA NAVAL\n");
            Console.WriteLine("Insere uma opcao");
            Console.WriteLine("1 - Jogo PvP (Player vs Player)");      
            Console.WriteLine("2 - Jogo PvPc (Player vs Pc)");
            Console.WriteLine("3 - Sair");
        }

        static void menu2()
        {
            Console.WriteLine("Insere uma opcao");
            Console.WriteLine("1 - Mivel Facil");
            Console.WriteLine("2 - Mivel Medio");
            Console.WriteLine("3 - Mivel Dificil");
            Console.WriteLine("4 - Voltar");
        }



        //ficheiro
        static void printScores(string nomeFicheiro, List<Jogador> lst_vencedores)
        {
            // leitura e escrita do ficheiro de leaderboard

            FileInfo ficheiro = new FileInfo($@"c:\ficheiros\{nomeFicheiro}.txt");


            StreamReader ler;

            try // tenta ler o ficheiro caso este exista 
            {
                ler = new StreamReader($@"c:\ficheiros\{nomeFicheiro}.txt");
                ler.Close();
            }

            catch (Exception ex) //se nao existir cria um ficheiro novo
            {
                FileStream fstr = ficheiro.Create();
                fstr.Close();
            }

            ler = new StreamReader($@"c:\ficheiros\{nomeFicheiro}.txt");

            string linha = null;

            while ((linha = ler.ReadLine()) != null) //leitura do ficheiro e colocaçao numa lista
            {

                Jogador jogador = new Jogador();
                jogador.nome = linha.Substring(6, linha.IndexOf(" - ") - 6);
                jogador.numeroJogadas = int.Parse(linha.Substring(linha.IndexOf(" - ") + 3));
                Console.WriteLine(jogador.nome);
                lst_vencedores.Add(jogador);

            }
            ler.Close();



            StreamWriter escrita = ficheiro.CreateText();

            // reescreve o ficheiro
            foreach (Jogador j in lst_vencedores.OrderBy(x => x.numeroJogadas))
            {

                escrita.WriteLine($"Nome: {j.nome} - {j.numeroJogadas}");
            }

            escrita.Close();
        }


        //criacao de nome
        static string setNome()
        {

            string nome;
            do
            {
                Console.WriteLine("Introduza no nome do jogador");
                nome = Console.ReadLine();
                if (nome.Length < 3)
                {
                    Console.WriteLine("Pelo menos 3 Letras ou Numeros");
                    Console.ReadLine();
                    Console.Clear();
                }

            }
            while (nome.Length < 3);
            return nome;

        }


    }


    class Jogador
    {
        public void criarJogador(string nome, int numeroJogadas, char [,] tabuleiroPecas, char [,] tabuleiroJogadas)
        {
            this.nome = nome;
            this.numeroJogadas = numeroJogadas;
            this.tabuleiroJogadas = tabuleiroJogadas;
            this.tabuleiroPecas = tabuleiroPecas;
        }

        public string nome { get; set; }

        public int numeroJogadas { get; set; }

        public char [,] tabuleiroPecas { get; set; }

        public char [,] tabuleiroJogadas { get; set; }



    }

    class Coordenada
    {
        public int linha { get; set; }
        public int coluna { get; set; }
        public void changeC() { }

    }
}
