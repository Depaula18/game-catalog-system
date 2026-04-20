import { useState } from 'react';
import { GameList } from './components/GameList';
import { GameModal } from './components/GameModal';

function App() {

  const [isModalOpen, setIsModalOpen] = useState(false);
  

  const [refreshList, setRefreshList] = useState(0);

  return (
    <div className="min-h-screen bg-gray-900 text-gray-100 font-sans">
      <header className="bg-gray-950 border-b border-gray-800 sticky top-0 z-10 shadow-md">
        <div className="max-w-7xl mx-auto px-4 py-5 sm:px-6 lg:px-8 flex justify-between items-center">
          <div className="flex items-center gap-3">
            <div className="w-8 h-8 bg-emerald-500 rounded-lg shadow-emerald-500/50 shadow-lg"></div>
            <h1 className="text-2xl font-black tracking-tight text-white">
              Game<span className="text-emerald-500">Catalog</span>
            </h1>
          </div>
          

          <button 
            onClick={() => setIsModalOpen(true)}
            className="bg-emerald-600 hover:bg-emerald-500 text-white px-5 py-2 rounded-lg font-semibold transition-colors shadow-lg shadow-emerald-600/30"
          >
            + Novo Jogo
          </button>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 py-8 sm:px-6 lg:px-8">
        <div className="mb-8">
          <h2 className="text-3xl font-bold text-white mb-2">Seu Catálogo</h2>
          <p className="text-gray-400">Gerencie todos os títulos da sua biblioteca.</p>
        </div>


        <GameList refreshTrigger={refreshList} />
      </main>


      <GameModal 
        isOpen={isModalOpen} 
        onClose={() => setIsModalOpen(false)} 
        onSuccess={() => setRefreshList(refreshList + 1)} 
      />
    </div>
  );
}

export default App;