import { useState } from 'react';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { Login } from './components/Login';
import { GameList } from './components/GameList';
import { LogOut, Plus } from 'lucide-react';
import { GameModal } from './components/GameModal';

interface Game {
  id: string;
  title: string;
  description: string;
  price: number;
  releaseDate: string;
  genreName: string;
  coverUrl?: string;
  genreId?: string;
}

function AppContent() {
  const { isAuthenticated, logout } = useAuth();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [gameToEdit, setGameToEdit] = useState<Game | null>(null);
  const [refreshListTrigger, setRefreshListTrigger] = useState(0);

  const handleEditGame = (game: Game) => {
    setGameToEdit(game);
    setIsModalOpen(true);
  };

  const handleOpenNewGame = () => {
    setGameToEdit(null);
    setIsModalOpen(true);
  };

  if (!isAuthenticated) {
    return <Login />;
  }

  return (
    <div className="min-h-screen bg-gray-900 text-white p-8">
      <div className="max-w-7xl mx-auto">
        <header className="flex justify-between items-center mb-8 bg-gray-800 p-6 rounded-2xl border border-gray-700 shadow-lg">
          <div>
            <h1 className="text-3xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-emerald-400 to-cyan-400">
              Catálogo de Jogos
            </h1>
            <p className="text-gray-400 mt-1">Gerenciamento Administrativo</p>
          </div>
          
          <div className="flex gap-4">
            {/* O BOTÃO DE NOVO JOGO */}
            <button 
              onClick={handleOpenNewGame}
              className="flex items-center gap-2 bg-emerald-600 hover:bg-emerald-500 text-white px-5 py-2.5 rounded-xl transition-all font-bold shadow-lg shadow-emerald-500/20 active:scale-95"
            >
              <Plus size={20} />
              <span className="hidden sm:inline">Novo Jogo</span>
            </button>

            {/* Botão de Logout */}
            <button 
              onClick={logout}
              className="flex items-center gap-2 bg-red-500/10 text-red-500 hover:bg-red-500 hover:text-white px-4 py-2.5 rounded-xl transition-all font-medium border border-red-500/20"
            >
              <LogOut size={18} />
              <span className="hidden sm:inline">Sair</span>
            </button>
          </div>
        </header>

        <GameList refreshTrigger={refreshListTrigger} onEdit={handleEditGame} />
        
        <GameModal
          isOpen={isModalOpen}
          gameToEdit={gameToEdit}
          onClose={() => setIsModalOpen(false)}
          onSuccess={() => setRefreshListTrigger((prev) => prev + 1)}
        />
      </div>
    </div>
  );
}

function App() {
  return (
    <AuthProvider>
      <AppContent />
    </AuthProvider>
  );
}

export default App;