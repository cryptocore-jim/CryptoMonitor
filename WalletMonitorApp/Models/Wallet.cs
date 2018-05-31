using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WalletMonitorApp.Helpers;
using WalletMonitorApp.Properties;

namespace WalletMonitorApp.Models
{
    public class Wallet : Caliburn.Micro.PropertyChangedBase
    {
        public Wallet()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += (sender, e) =>
            {
                NotifyOfPropertyChange(() => LastUpdatedStr);
            };
            timer.Start();
        }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                NotifyOfPropertyChange(() => Selected);
            }
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                NotifyOfPropertyChange(() => Address);
                NotifyOfPropertyChange(() => ExplorerURLString);
            }
        }

        private string _coinSymbol;
        public string CoinSymbol
        {
            get
            {
                return _coinSymbol;
            }
            set
            {
                _coinSymbol = value;
                NotifyOfPropertyChange(() => CoinSymbol);
            }
        }


        private string _coinName;
        public string CoinName
        {
            get
            {
                return _coinName;
            }
            set
            {
                _coinName = value;
                NotifyOfPropertyChange(() => CoinName);
                NotifyOfPropertyChange(() => CoinNamePreview);
            }
        }
        
        public string CoinNamePreview
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(CoinName);
                if (Degraded)
                {
                    sb.Append(" (Degraded)");
                }
                return sb.ToString();
            }
        }

        private decimal? _priceBTC;
        public decimal? PriceBTC
        {
            get
            {
                if (_priceBTC != null)
                {
                    return CurrencyHelper.BTCHelperDecimal(_priceBTC.Value);
                }
                return _priceBTC;
            }
            set
            {
                _priceBTC = value;
                NotifyOfPropertyChange(() => PriceBTC);
            }
        }

        private decimal? _trend1;
        public decimal? Trend1
        {
            get
            {
                return _trend1;
            }
            set
            {
                _trend1 = value;
                NotifyOfPropertyChange(() => Trend1);
            }
        }

        private decimal? _trend7;
        public decimal? Trend7
        {
            get
            {
                return _trend7;
            }
            set
            {
                _trend7 = value;
                NotifyOfPropertyChange(() => Trend7);
            }
        }

        private decimal? _trend14;
        public decimal? Trend14
        {
            get
            {
                return _trend14;
            }
            set
            {
                _trend14 = value;
                NotifyOfPropertyChange(() => Trend14);
            }
        }

        private decimal? _amount;
        public decimal? Amount
        {
            get
            {
                if (_amount != null)
                {
                    return CurrencyHelper.BTCHelperDecimal(_amount.Value);
                }
                return _amount;
            }
            set
            {
                _amount = value;
                NotifyOfPropertyChange(() => Amount);
            }
        }

        private decimal? _amountBTC;
        public decimal? AmountBTC
        {
            get
            {
                if (_amountBTC != null)
                {
                    return CurrencyHelper.BTCHelperDecimal(_amountBTC.Value);
                }
                return _amountBTC;
            }
            set
            {
                _amountBTC = value;
                NotifyOfPropertyChange(() => AmountBTC);
            }
        }

        private decimal? _amountUSD;
        public decimal? AmountUSD
        {
            get
            {
                return _amountUSD;
            }
            set
            {
                _amountUSD = value;
                NotifyOfPropertyChange(() => AmountUSD);
            }
        }

        private decimal? _amountEUR;
        public decimal? AmountEUR
        {
            get
            {
                return _amountEUR;
            }
            set
            {
                _amountEUR = value;
                NotifyOfPropertyChange(() => AmountEUR);
            }
        }

        private long _lastUpdated;
        public long LastUpdated
        {
            get
            {
                return _lastUpdated;
            }
            set
            {
                _lastUpdated = value;
                NotifyOfPropertyChange(() => LastUpdated);
                NotifyOfPropertyChange(() => LastUpdatedStr);
            }
        }

        public string LastUpdatedStr
        {
            get
            {
                if (LastUpdated == 0)
                {
                    return Resources.NA;
                }
                return TimeHelper.ToAge(LastUpdated) + " ago";
            }

        }

        private bool _degraded;
        public bool Degraded
        {
            get
            {
                return _degraded;
            }
            set
            {
                _degraded = value;
                NotifyOfPropertyChange(() => Degraded);
                NotifyOfPropertyChange(() => CoinNamePreview);
            }
        }

        private string _explorerURL;
        public string ExplorerURL
        {
            get
            {
                return _explorerURL;
            }
            set
            {
                _explorerURL = value;
                NotifyOfPropertyChange(() => ExplorerURL);
                NotifyOfPropertyChange(() => ExplorerURLString);
            }
        }

        public string ExplorerURLString
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(ExplorerURL);
                sb.Append("address/");
                sb.Append(Address);
                return sb.ToString();
            }
        }

        private bool? _isMasternode;
        public bool? IsMasternode
        {
            get
            {
                return _isMasternode;
            }
            set
            {
                _isMasternode = value;
                NotifyOfPropertyChange(() => IsMasternode);
                NotifyOfPropertyChange(() => MasternodeStatus);
            }
        }

        private string _masternodeStatus;
        public string MasternodeStatus
        {
            get
            {
                if (_isMasternode == null)
                {
                    return Resources.NA;
                }
                if (!_isMasternode.Value)
                {
                    return Resources.NotMasternode;
                }
                return _masternodeStatus;
            }
            set
            {
                _masternodeStatus = value;
                NotifyOfPropertyChange(() => MasternodeStatus);
                NotifyOfPropertyChange(() => IsMasternode);
            }
        }
    }
}
