import { useState } from 'react';
import type { FormEvent } from 'react';
import { Mail, Lock, Gamepad2, ArrowRight } from 'lucide-react';
import { api } from '../services/api';
import { useAuth } from '../contexts/AuthContext';

export function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const { login } = useAuth();

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    try {
      const response = await api.post('/Auth/login', {
        email,
        password
      });

      const token = response.data.token;
      login(token);
    } catch {
      setError('Credenciais invalidas. Verifique seu e-mail e senha.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-900 flex items-center justify-center p-4">
      
      {/* O Card Central de Login */}
      <div className="bg-gray-800 border border-gray-700 rounded-2xl p-8 w-full max-w-md shadow-2xl relative overflow-hidden">
        
        <div className="absolute top-0 left-1/2 -translate-x-1/2 w-32 h-32 bg-emerald-500/20 rounded-full blur-3xl pointer-events-none"></div>

        {/* Cabeçalho */}
        <div className="flex flex-col items-center mb-8 relative z-10">
          <div className="bg-gray-900 p-3 rounded-xl border border-gray-700 mb-4 shadow-inner">
            <Gamepad2 size={32} className="text-emerald-500" />
          </div>
          <h2 className="text-3xl font-bold text-white mb-1">Acesso Restrito</h2>
          <p className="text-gray-400 text-sm">Insira suas credenciais para gerenciar o catálogo.</p>
        </div>

        {/* Formulário */}
        <form onSubmit={handleSubmit} className="space-y-5 relative z-10">
          {error && (
            <div className="bg-red-500/10 border border-red-500 text-red-500 text-sm p-3 rounded-lg text-center">
              {error}
            </div>
          )}
          
          {/* Input de E-mail */}
          <div className="space-y-1">
            <label className="text-sm font-medium text-gray-300 ml-1">E-mail Corporativo</label>
            <div className="relative">
              <Mail className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500" size={20} />
              <input 
                type="email"
                required
                placeholder="admin@catalogo.com"
                className="w-full bg-gray-900 border border-gray-700 rounded-xl py-3 pl-10 pr-4 text-white focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500 outline-none transition-all placeholder:text-gray-600"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
          </div>

          {/* Input de Senha */}
          <div className="space-y-1">
            <label className="text-sm font-medium text-gray-300 ml-1">Senha de Acesso</label>
            <div className="relative">
              <Lock className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500" size={20} />
              <input 
                type="password"
                required
                placeholder="••••••••"
                className="w-full bg-gray-900 border border-gray-700 rounded-xl py-3 pl-10 pr-4 text-white focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500 outline-none transition-all placeholder:text-gray-600"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>

          {/* Botão de Submit */}
          <button 
            type="submit"
            disabled={isLoading}
            className="w-full bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 text-white font-bold py-3.5 px-4 rounded-xl flex items-center justify-center gap-2 transition-all duration-300 shadow-lg shadow-emerald-500/20 active:scale-[0.98] mt-4"
          >
            <span>{isLoading ? 'Autenticando...' : 'Entrar no Sistema'}</span>
            <ArrowRight size={20} />
          </button>
        </form>

      </div>
    </div>
  );
}