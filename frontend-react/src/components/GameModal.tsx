import { useState, useEffect } from 'react';
import { api } from '../services/api';

interface Genre {
  id: string;
  name: string;
}

interface GameModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
  gameToEdit?: any; 
}

export function GameModal({ isOpen, onClose, onSuccess, gameToEdit }: GameModalProps) {
  const [genres, setGenres] = useState<Genre[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  

  const [formData, setFormData] = useState({
    title: '',
    description: '',
    price: '',
    releaseDate: '',
    genreId: ''
  });


  useEffect(() => {
    if (isOpen) {
      api.get('/api/Genres').then(response => setGenres(response.data));
    }
  }, [isOpen]);


  useEffect(() => {
    if (gameToEdit && isOpen) {
      setFormData({
        title: gameToEdit.title,
        description: gameToEdit.description,
        price: gameToEdit.price.toString(),
        releaseDate: gameToEdit.releaseDate.split('T')[0], 
        genreId: gameToEdit.genreId
      });
    } else {
      setFormData({ title: '', description: '', price: '', releaseDate: '', genreId: '' });
    }
  }, [gameToEdit, isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); 
    setIsLoading(true);

    try {
      const dataToSend = { ...formData, price: Number(formData.price) };

      if (gameToEdit) {
        await api.put(`/api/Games/${gameToEdit.id}`, dataToSend);
        alert('Jogo atualizado com sucesso!');
      } else {
        await api.post('/api/Games', dataToSend);
        alert('Jogo salvo com sucesso!');
      }
      
      onSuccess(); 
      onClose();  
    } catch (error) {
      console.error(error);
      alert('Erro ao salvar o jogo. Verifique os dados.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/70 backdrop-blur-sm p-4 animate-in fade-in duration-200">
      <div className="bg-gray-800 rounded-2xl w-full max-w-md p-6 border border-gray-700 shadow-2xl">
        <h2 className="text-2xl font-bold text-white mb-6">
          {gameToEdit ? 'Editar Jogo' : 'Adicionar Novo Jogo'}
        </h2>
        
        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          
          <div>
            <label className="text-gray-400 text-sm mb-1 block">Título do Jogo</label>
            <input required type="text" 
              value={formData.title}
              className="w-full bg-gray-900 text-white rounded-lg p-3 border border-gray-700 focus:border-emerald-500 focus:outline-none"
              onChange={(e) => setFormData({...formData, title: e.target.value})} 
            />
          </div>

          <div>
            <label className="text-gray-400 text-sm mb-1 block">Gênero</label>
            <select required 
              value={formData.genreId}
              className="w-full bg-gray-900 text-white rounded-lg p-3 border border-gray-700 focus:border-emerald-500 focus:outline-none"
              onChange={(e) => setFormData({...formData, genreId: e.target.value})}
            >
              <option value="" disabled>Selecione um gênero...</option>
              {genres.map(g => (
                <option key={g.id} value={g.id}>{g.name}</option>
              ))}
            </select>
          </div>

          <div className="flex gap-4">
            <div className="flex-1">
              <label className="text-gray-400 text-sm mb-1 block">Preço (R$)</label>
              <input required type="number" step="0.01" min="0"
                value={formData.price}
                className="w-full bg-gray-900 text-white rounded-lg p-3 border border-gray-700 focus:border-emerald-500 focus:outline-none"
                onChange={(e) => setFormData({...formData, price: e.target.value})} 
              />
            </div>
            <div className="flex-1">
              <label className="text-gray-400 text-sm mb-1 block">Lançamento</label>
              <input required type="date" 
                value={formData.releaseDate}
                className="w-full bg-gray-900 text-white rounded-lg p-3 border border-gray-700 focus:border-emerald-500 focus:outline-none"
                onChange={(e) => setFormData({...formData, releaseDate: e.target.value})} 
                style={{ colorScheme: 'dark' }}
              />
            </div>
          </div>

          <div>
            <label className="text-gray-400 text-sm mb-1 block">Descrição</label>
            <textarea required rows={3}
              value={formData.description}
              className="w-full bg-gray-900 text-white rounded-lg p-3 border border-gray-700 focus:border-emerald-500 focus:outline-none resize-none"
              onChange={(e) => setFormData({...formData, description: e.target.value})} 
            ></textarea>
          </div>

          <div className="flex justify-end gap-3 mt-4">
            <button type="button" onClick={onClose} 
              className="px-4 py-2 text-gray-400 hover:text-white transition-colors">
              Cancelar
            </button>
            <button type="submit" disabled={isLoading}
              className="px-6 py-2 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white rounded-lg font-semibold transition-colors shadow-lg shadow-emerald-600/30">
              {isLoading ? 'Salvando...' : (gameToEdit ? 'Salvar Alterações' : 'Salvar Jogo')}
            </button>
          </div>
        </form>

      </div>
    </div>
  );
}