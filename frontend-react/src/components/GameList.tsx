import { useEffect, useState } from 'react';
import { api } from '../services/api';
import { Trash2, Pencil, Search, ChevronLeft, ChevronRight } from 'lucide-react';

interface Game {
  id: string;
  title: string;
  description: string;
  price: number;
  releaseDate: string;
  genreName: string;
}


interface PagedResponse {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  data: Game[];
}

export function GameList({ refreshTrigger, onEdit }: { refreshTrigger: number, onEdit: (game: Game) => void }) {
  const [gameData, setGameData] = useState<PagedResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [expandedIds, setExpandedIds] = useState<string[]>([]);

 
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 6; 

  const fetchGames = async () => {
    try {
      setLoading(true);

      const response = await api.get('/Games', {
        params: {
          page: currentPage,
          pageSize: pageSize,
          search: searchTerm
        }
      });
      setGameData(response.data);
    } catch (error) {
      console.error("Erro ao buscar jogos:", error);
    } finally {
      setLoading(false);
    }
  };


  useEffect(() => {
    fetchGames();
  }, [refreshTrigger, currentPage, searchTerm]);


  const handleSearchChange = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1);
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm("Deseja realmente excluir este jogo?")) return;
    try {
      await api.delete(`/Games/${id}`);
      fetchGames(); 
      alert("Jogo removido!");
    } catch (error) {
      alert("Erro ao excluir.");
    }
  };

  const toggleDescription = (id: string) => {
    setExpandedIds(prev => prev.includes(id) ? prev.filter(i => i !== id) : [...prev, id]);
  };

  return (
    <div className="space-y-6">
      
      {/* BARRA DE BUSCA */}
      <div className="relative max-w-md">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500" size={20} />
        <input 
          type="text"
          placeholder="Buscar por título ou descrição..."
          className="w-full bg-gray-800 border border-gray-700 rounded-xl py-3 pl-10 pr-4 text-white focus:border-emerald-500 outline-none transition-all"
          value={searchTerm}
          onChange={(e) => handleSearchChange(e.target.value)}
        />
      </div>

      {loading ? (
        <div className="text-center py-20 text-gray-400">Sincronizando biblioteca...</div>
      ) : (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {gameData?.data.length === 0 ? (
              <p className="text-gray-400 col-span-full text-center py-10">Nenhum jogo encontrado.</p>
            ) : (
              gameData?.data.map((game) => {
                const isExpanded = expandedIds.includes(game.id);
                return (
                  <div key={game.id} className="flex flex-col bg-gray-800 rounded-xl p-6 border border-gray-700 shadow-lg hover:border-emerald-500 transition-colors duration-300">
                    <div className="flex justify-between items-start mb-4">
                      <h3 className="text-xl font-bold text-white truncate pr-2" title={game.title}>{game.title}</h3>
                      <span className="shrink-0 bg-emerald-500/10 text-emerald-400 text-xs px-2 py-1 rounded-full border border-emerald-500/20">
                        {game.genreName}
                      </span>
                    </div>
                    
                    <div onClick={() => toggleDescription(game.id)} className="cursor-pointer group mb-6 flex-1">
                      <p className={`text-gray-400 text-sm transition-all ${isExpanded ? '' : 'line-clamp-2'}`}>
                        {game.description}
                      </p>
                    </div>
                    
                    <div className="flex justify-between items-end pt-4 border-t border-gray-700/50">
                      <div className="flex flex-col">
                        <span className="text-lg font-bold text-emerald-400">
                          {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(game.price)}
                        </span>
                        <span className="text-[10px] text-gray-500">
                          {new Date(game.releaseDate).toLocaleDateString('pt-BR', { timeZone: 'UTC' })}
                        </span>
                      </div>

                      <div className="flex gap-1">
                        <button onClick={() => onEdit(game)} className="p-2 text-gray-500 hover:text-emerald-500 hover:bg-emerald-500/10 rounded-lg transition-colors">
                          <Pencil size={18} />
                        </button>
                        <button onClick={() => handleDelete(game.id)} className="p-2 text-gray-500 hover:text-red-500 hover:bg-red-500/10 rounded-lg transition-colors">
                          <Trash2 size={18} />
                        </button>
                      </div>
                    </div>
                  </div>
                );
              })
            )}
          </div>

          {/* CONTROLES DE PAGINAÇÃO */}
          {gameData && gameData.totalPages > 1 && (
            <div className="flex justify-center items-center gap-4 pt-8">
              <button 
                disabled={currentPage === 1}
                onClick={() => setCurrentPage(prev => prev - 1)}
                className="p-2 rounded-lg bg-gray-800 border border-gray-700 disabled:opacity-30 disabled:cursor-not-allowed hover:bg-gray-700 transition-colors"
              >
                <ChevronLeft size={24} />
              </button>
              
              <span className="text-gray-400 font-medium">
                Página <span className="text-white">{gameData.currentPage}</span> de <span className="text-white">{gameData.totalPages}</span>
              </span>

              <button 
                disabled={currentPage === gameData.totalPages}
                onClick={() => setCurrentPage(prev => prev + 1)}
                className="p-2 rounded-lg bg-gray-800 border border-gray-700 disabled:opacity-30 disabled:cursor-not-allowed hover:bg-gray-700 transition-colors"
              >
                <ChevronRight size={24} />
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
}