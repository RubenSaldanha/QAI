using System;
using System.Collections.Generic;

namespace QAI.AIS.Irektu
{
    class Irekt : QuatroPlayer
    {
        private byte[,] byte_field;
        private QuatroField field_copy;
        private int depth = 3;

        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            generate_byte_field(field);
            field_copy = field;
            Node root = new Node(1,0,null,int.MaxValue,byte_field,get_heads(field_copy));
            int play = alpha_beta(root, depth, int.MinValue, int.MaxValue, true).Item1;
            Console.WriteLine(String.Format("Playing to {0}",play));
            return play;
        }

        // Create a copy of the field and store it as 8-bit matrix. It can be used to caculate the value of a play.
        private void generate_byte_field(QuatroField field)
        {
            byte_field = new byte[7, 9];
        	//  Field has 7 rows , 9 columns
        	for(int I = 0; I < 7; I++)
        	{
        		for(int J = 0; J < 9; J++)
        		{
                    // Player 1 is seen as 1
        			if(field[I,J] == 1) 
        			{
	        			byte_field[I,J] = 1;
        			}
                    // Player 2 is seen as 2
        			else if(field[I,J] == 2) 
        			{
	        			byte_field[I,J] = 2;        				
        			}
                    else{
                        byte_field[I,J] = 0;
                    }
        		}
        	}
        }

        // alpha-beta pruning algorithm based on minmax algorithm
        // First call : minimax(root_node, depth, Math.MinValue, Math.MaxValue,true)

        private Tuple<int,int> alpha_beta(Node node, int depth, int alpha, int beta, bool maxim_player)
        {
            int best_value;
            List<Node> children = successors(node);

            // Node is  root and there or no more children to check, return best play 
            if (depth == 0 || children == null)
            {
                return new Tuple<int, int>(node.Get_play(), eval(node));
            }
            if (maxim_player)
            {
                best_value = int.MinValue;
                foreach(Node n in children)
                {
                   best_value = Math.Max(best_value,alpha_beta(n,depth-1,alpha,beta,false).Item2);
                   alpha = Math.Max(alpha,best_value);
                   if(beta <= alpha)
                   {
                    break;
                   }  
                }
                return new Tuple<int,int>(node.Get_play(),best_value);
            }
            else
            {
                best_value = int.MaxValue;
                foreach(Node n in children)
                {
                   best_value = Math.Min(best_value,alpha_beta(n,depth-1,alpha,beta,true).Item2);
                   alpha = Math.Min(alpha,best_value);
                   if(beta <= alpha)
                   {
                    break;
                   }  
                }
                return new Tuple<int,int>(node.Get_play(),best_value);
            }
        }

        // Retrieve sucessors Nodes from a given Node.
        private List<Node> successors(Node n_node)
        {
            if(n_node.Get_depth() == depth)
            {
                return null;
            }
            List<int> possible_actions = actions(n_node);
            List<Node> node_successors = new List<Node>();

            foreach(int action in possible_actions)
            {
                node_successors.Add(result(action,n_node));
            }

            return node_successors;
        }

        // Retrieves the value of a given Node. Should return +INF if I win , -INF if enemy wins.
        private int eval(Node n_node)
        {
            // TODO Calculate each tile value in range [-3;3] where :
            //                                              - 3 is Enemy 3 Sequence. 
            //                                              + 3 is Ally 3 Sequence.
            // There must be checked:
            //              vertical
            //              horizontal
            //              diagonal (+1)
            //              diagonal (-1)
            return 1;
        }

        // Retrieves all legal plays from a given Node.
        private List<int> actions(Node n_node)
        {
            List<int> legal_actions = new List<int>();
            for(int column = 0; column < 9; column ++)
            {
                if(canPlay(column,n_node))
                    legal_actions.Add(column);
            }
            return legal_actions;
        }

        // Calculates the resulting Node of applying an action to a Node
        private Node result(int action, Node n_node)
        {
            Node successor;
            byte successor_player;

            if(n_node.Get_player() == 1)
                successor_player = 2;
            else
                successor_player = 1;

            int[] successor_heads = (int[]) n_node.Get_heads().Clone();
            successor_heads[action] += 1;

            byte[,] successor_game_state = (byte[,]) n_node.Get_game_state().Clone();

            successor_game_state[successor_heads[action],action] = n_node.Get_player();

            successor = new Node(   successor_player,
                                    n_node.Get_depth() + 1,
                                    n_node,
                                    action,
                                    successor_game_state,
                                    successor_heads);
            return successor;   
        } 

        // Verifies if a given action on a Node is legal
        private bool canPlay(int action, Node n_node)
        {
            return n_node.Get_heads()[action] < 7;
        }

        // Copies a byte matrix
        private Byte[,] copy_matrix(byte[,] original_field)
        {
            byte[,] copy = new Byte[7,9];
            for(int I = 0; I < 7; I++)
            {
                for(int J = 0; J < 9; J++)
                {
                        copy[I,J] = original_field[I,J];
                }
            }
            return copy;
        }

        // Retrieves heads from a given QuatroField
        private int[] get_heads(QuatroField field)
        {
            int[] heads = new int[9];
            for(int I = 0; I < 9; I++)
            {
                heads[I] = field.head(I);
            }
            return heads;
        }
    }


    


    // Represents a Node from the minmax tree
    class Node
    {
    	int depth;
        Node parent;
        int play;
        byte[,] game_state;
        byte player;
        int[] game_state_heads;

    	public Node(byte b_player,int i_depth, Node n_parent,int i_play, byte[,] b_game_state, int[] heads)
    	{
            player = b_player;
    		depth = i_depth;
            parent = n_parent;
            play = i_play;
            game_state = b_game_state; 
            game_state_heads = heads;
    	}

        public int Get_depth()
        {
            return depth;
        }

        public void Set_depth(int i_depth)
        {
            depth = i_depth;
        }

        public Node Get_parent()
        {
            return parent;
        }

        public void Set_parent(Node n_parent)
        {
            parent = n_parent;
        }

        public int Get_play()
        {
            return play;
        }

        public void Set_play(int i_play)
        {
            play = i_play;
        }

        public byte[,] Get_game_state()
        {
            return game_state;
        }

        public byte Get_player()
        {
            return player;
        }

        public int[] Get_heads()
        {
            return game_state_heads;
        }
    }
}
