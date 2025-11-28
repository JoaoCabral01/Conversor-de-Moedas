using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Conversor.Models;
using Conversor.Services;
using Conversor.Helpers;
using System.Runtime.CompilerServices;
using System.ComponentModel;

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

    }
}


