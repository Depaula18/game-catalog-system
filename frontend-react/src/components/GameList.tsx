import { useEffect, useState } from 'react';
import { api } from '../services/api';
import { Trash2 } from 'lucide-react';

interface Game {
  id: string;
  title: string;
  description: string;
  price: number;
  releaseDate: string;
  genreName: string;
}

export function GameList({ refreshTrigger }: { refreshTrigger: number }) {
  const [games, setGames] = useState<Game[]>([]);
  const [loading, setLoading] = useState(true);
  
  // NOVO: Estado para guardar os IDs dos jogos que estão com a descrição expandida
  const [expandedIds, setExpandedIds] = useState<string[]>([]);

  const fetchGames = async () => {
    try {
      const response = await api.get('/Games');
      setGames(response.data);
    } catch (error) {
      console.error("Erro ao buscar jogos:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchGames();
  }, [refreshTrigger]);

  const handleDelete = async (id: string) => {
    if (!window.confirm("Deseja realmente excluir este jogo?")) return;

    try {
      await api.delete(`/Games/${id}`);
      setGames(games.filter(game => game.id !== id));
      alert("Jogo removido com sucesso!");
    } catch (error) {
      console.error("Erro ao excluir:", error);
      alert("Não foi possível excluir o jogo.");
    }
  };

  // NOVO: Função que liga/desliga a expansão da descrição
  const toggleDescription = (id: string) => {
    setExpandedIds(prev => 
      prev.includes(id) 
        ? prev.filter(gameId => gameId !== id) // Se já está expandido, remove da lista (recolhe)
        : [...prev, id]                        // Se não está, adiciona na lista (expande)
    );
  };

  if (loading) {
    return <div className="text-center mt-10 text-xl text-gray-400">Carregando arsenal...</div>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {games.length === 0 ? (
        <p className="text-gray-400 col-span-full text-center">Nenhum jogo cadastrado ainda.</p>
      ) : (
        games.map((game) => {
          // NOVO: Verifica se o ID deste jogo específico está na lista de expandidos
          const isExpanded = expandedIds.includes(game.id);

          return (
            <div key={game.id} className="flex flex-col bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg hover:border-emerald-500 transition-colors duration-300">
              
              <div className="flex justify-between items-start mb-4">
                <h3 className="text-2xl font-bold text-white">{game.title}</h3>
                <span className="bg-emerald-500/10 text-emerald-400 text-sm px-3 py-1 rounded-full border border-emerald-500/20">
                  {game.genreName}
                </span>
              </div>
              
              {/* ATUALIZADO: A descrição agora é clicável e alterna a classe line-clamp */}
              <div 
                onClick={() => toggleDescription(game.id)}
                className="cursor-pointer group mb-6"
                title={isExpanded ? "Clique para recolher" : "Clique para ler mais"}
              >
                <p className={`text-gray-400 text-sm transition-all duration-300 ${isExpanded ? '' : 'line-clamp-2'}`}>
                  {game.description}
                </p>
                {/* Um pequeno feedback visual para o usuário saber que é clicável */}
                <span className="text-xs text-emerald-500/0 group-hover:text-emerald-500/80 transition-colors">
                  {isExpanded ? "Recolher" : "Ler mais..."}
                </span>
              </div>
              
              <div className="flex justify-between items-end mt-auto pt-4 border-t border-gray-700/50">
                <div className="flex flex-col">
                  <span className="text-xl font-bold text-emerald-400">
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(game.price)}
                  </span>
                  <span className="text-xs text-gray-500 mt-1">
                    Lançamento: {new Date(game.releaseDate).toLocaleDateString('pt-BR', { timeZone: 'UTC' })}
                  </span>
                </div>

                <button 
                  onClick={() => handleDelete(game.id)}
                  title="Excluir Jogo"
                  className="p-2 text-gray-500 hover:text-red-500 hover:bg-red-500/10 rounded-lg transition-colors"
                >
                  <Trash2 size={20} />
                </button>
              </div>

            </div>
          );
        })
      )}
    </div>
  );
}