using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Conversor.Models;
using Conversor.Services;
using Conversor.Helpers;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Conversor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BancoDeDadosService _db;
        public ObservableCollection<Conversao> Conversaos { get; } = new();

        public string[] moedas { get; } = new string[]
        {
            "USD",
            "EUR",
            "GBP",
            "JPY",
            "BRL"
        };

        private string _moedaOrigem = "BRL";
        public string MoedaOrigem { get => _moedaOrigem; set { _moedaOrigem = value; OnPropertyChanged(); } }

        public string _moedaDestino = "USD";
        public string MoedaDestino { get => _moedaDestino; set { _moedaDestino = value; OnPropertyChanged(); } }

        private double _valor = 1.0;
        public double Valor { get => _valor; set { _valor = value; OnPropertyChanged(); } }

        private double _resultado;
        public double Resultado { get => _resultado; set { _resultado = value; OnPropertyChanged(); } }

        private Conversao? _selecionado;
        public Conversao? Selecionado
        {
            get => _selecionado;
            set
            {
                _selecionado = value;
                OnPropertyChanged();
                if (value != null)
                {
                    MoedaOrigem = value.MoedaOrigem;
                    MoedaDestino = value.MoedaDestino;
                    Valor = value.Valor;
                    Resultado = value.Resultado;
                }
            }
        }

        public ICommand ConverterCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand NovoCommand { get; }


        public event PropertyChangedEventHandler? PropertyChanged;


        public MainViewModel()
        {
            var caminhoDB = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "conversor.db"
            );
            _db = new BancoDeDadosService(caminhoDB);
            Carregar();

            ConverterCommand = new RelayCommand(_ => Converter());
            SalvarCommand = new RelayCommand(_ => Salvar());
            ExcluirCommand = new RelayCommand(_ => Excluir(), _ => Selecionado != null);
            NovoCommand = new RelayCommand(_ => Novo());

        }

        private void Carregar()
        {
            Conversaos.Clear();
            foreach (var iten in _db.ObterTodos())
                Conversaos.Add(iten);
        }

        private void Converter()
        {
            Resultado = Math.Round(Valor * ObterTaxa(MoedaOrigem, MoedaDestino), 4);
        }

        public void Salvar()
        {
            if (Selecionado != null)
            {
                Selecionado.MoedaOrigem = MoedaOrigem;
                Selecionado.MoedaDestino = MoedaDestino;
                Selecionado.Valor = Valor;
                Selecionado.Resultado = Resultado;
                _db.Atualizar(Selecionado);
            }
            else
            {
                var novo = new Conversao
                {
                    MoedaOrigem = MoedaOrigem,
                    MoedaDestino = MoedaDestino,
                    Valor = Valor,
                    Resultado = Resultado,
                    Criadoem = DateTime.UtcNow
                };
                novo.Id = _db.Inserir(novo);
                Conversaos.Insert(0, novo);
            }
        }

        public void Excluir()
        {
            if (Selecionado != null)
            {
                _db.Excluir(Selecionado.Id);
                Conversaos.Remove(Selecionado);
                Selecionado = null;
            }
        }

        public void Novo()
        {
            Selecionado = null;
            MoedaOrigem = "BRL";
            MoedaDestino = "USD";
            Valor = 1;
            Resultado = 0;
        }

        private double ObterTaxa(string origem, string destino)
        {
            if (origem == "BRL" && destino == "USD") return 0.18;
            if (origem == "USD" && destino == "BRL") return 5.60;

            if (origem == "BRL" && destino == "EUR") return 0.16;
            if (origem == "EUR" && destino == "BRL") return 6.10;

            return 1;
        }

        private void OnPropertyChanged([CallerMemberName] string? nome = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
        }
    }
}


