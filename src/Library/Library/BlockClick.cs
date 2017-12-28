using System.Threading.Tasks;

namespace Library.Library
{
    public class BlockClick
    {
        private readonly int _antialias;

        public BlockClick(int antialias)
        {
            _antialias = antialias;
        }


        public bool IsClicHot { get; set; } = false; 
        public async void BlockClickStart()
        {
            IsClicHot = true;
            await Task.Delay(_antialias);
            IsClicHot = false;
        }
    }
}