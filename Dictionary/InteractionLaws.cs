﻿using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public abstract class InteractionLaws
    {
        public List<string> GeneralGuidelines { get; private set; }
        public List<Tru> TatcaTru { get; private set; }

        public List<Tru> TuTru { get; private set; }

        protected void Init(TuTruMap ttm)
        {
            var laso = ttm.LaSoCuaToi;
            TatcaTru = new List<Tru>();
            TuTru = new List<Tru>();

            TuTru.AddRange(laso.TuTru.Values.ToList<Tru>());

            TatcaTru.AddRange(laso.TuTru.Values.ToList<Tru>());
            TatcaTru.Add(ttm.DaiVanHienTai);
            TatcaTru.Add(LookUpTable.TruOfTheYear());
        }

        abstract public void SetLaw();
    }

    public class DiaChiLucHop : InteractionLaws
    {
        public DiaChiLucHop(TuTruMap ttm)
        {
            base.Init(ttm);
        }

        public override void SetLaw()
        {
            this.CheckLucHop(ChiEnum.Ti, ChiEnum.Suu);
            this.CheckLucHop(ChiEnum.Dan, ChiEnum.Hoi);
            this.CheckLucHop(ChiEnum.Mao, ChiEnum.Tuat);
            
            this.CheckLucHop(ChiEnum.Thin, ChiEnum.Dau);
            this.CheckLucHop(ChiEnum.Ty, ChiEnum.Than);
            this.CheckLucHop(ChiEnum.Ngo, ChiEnum.Mui);
        }

        private void CheckLucHop(ChiEnum chi1, ChiEnum chi2)
        {
            var chi1Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi1);
            var chi2Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi2);

            if (chi1Id != -1 && chi2Id != -1)
            {
                var diaChi1 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi1);
                var diaChi2 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi2);
                var nguHanhChi1 = diaChi1.BanKhi.NguHanh;
                var nguHanhChi2 = diaChi2.BanKhi.NguHanh;

                var sinhKhac = LookUpTable.NguHanhSinhKhac[nguHanhChi1];
                var nhSinh = sinhKhac.Item1;
                var nhDuocSinh = sinhKhac.Item2;
                var nhKhac = sinhKhac.Item3;
                var nhBiKhac = sinhKhac.Item4;

                string thuocTinh = string.Empty;
                if (nguHanhChi2 == nhSinh || nguHanhChi2 == nhDuocSinh)
                {
                    thuocTinh = Constants.ThuocTinh.LUC_HOP_SINH;
                }
                else if (nguHanhChi2 == nhKhac || nguHanhChi2 == nhBiKhac)
                {
                    thuocTinh = Constants.ThuocTinh.LUC_HOP_KHAC;
                }

                var lucHop = NguHanhEnum.None;
                var lucHopTho = new List<ChiEnum> { ChiEnum.Ti, ChiEnum.Suu, ChiEnum.Ngo, ChiEnum.Mui };
                var lucHopMoc = new List<ChiEnum> { ChiEnum.Dan, ChiEnum.Hoi };
                var lucHopHoa = new List<ChiEnum> { ChiEnum.Mao, ChiEnum.Tuat };
                var lucHopKim = new List<ChiEnum> { ChiEnum.Thin, ChiEnum.Dau };
                var lucHopThuy = new List<ChiEnum> { ChiEnum.Ty, ChiEnum.Than };
                if (lucHopTho.Contains(chi1))
                {
                    lucHop = NguHanhEnum.Tho;
                }
                else if (lucHopMoc.Contains(chi1))
                {
                    lucHop = NguHanhEnum.Moc;
                }
                else if (lucHopHoa.Contains(chi1))
                {
                    lucHop = NguHanhEnum.Hoa;
                }
                else if (lucHopKim.Contains(chi1))
                {
                    lucHop = NguHanhEnum.Kim;
                }
                else if (lucHopThuy.Contains(chi1))
                {
                    lucHop = NguHanhEnum.Thuy;
                }

                if (!diaChi1.ThuocTinh.Keys.Contains(thuocTinh))
                {
                    diaChi1.ThuocTinh.Add(thuocTinh, lucHop);
                }

                if (!diaChi2.ThuocTinh.Keys.Contains(thuocTinh))
                {
                    diaChi2.ThuocTinh.Add(thuocTinh, lucHop);
                }

            }
        }
    }

    public class DiaChiLucXung : InteractionLaws
    {
        public DiaChiLucXung(TuTruMap ttm)
        {
            base.Init(ttm);
        }

        public override void SetLaw()
        {
            this.CheckLucXung(ChiEnum.Ti, ChiEnum.Ngo);
            this.CheckLucXung(ChiEnum.Suu, ChiEnum.Mui);
            this.CheckLucXung(ChiEnum.Dan, ChiEnum.Than);

            this.CheckLucXung(ChiEnum.Mao, ChiEnum.Dau);
            this.CheckLucXung(ChiEnum.Thin, ChiEnum.Tuat);
            this.CheckLucXung(ChiEnum.Ty, ChiEnum.Hoi);
        }

        private void CheckLucXung(ChiEnum chi1, ChiEnum chi2)
        {
            var chi1Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi1);
            var chi2Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi2);

            if (chi1Id != -1 && chi2Id != -1)
            {
                var diaChi1 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi1);
                var diaChi2 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi2);
               
                string thuocTinh = Constants.ThuocTinh.LUC_XUNG;
         
                var lucXung = string.Empty;
                var tiNgo = new List<ChiEnum> { ChiEnum.Ti, ChiEnum.Ngo};
                var suuMui = new List<ChiEnum> { ChiEnum.Suu, ChiEnum.Mui };
                var danThan = new List<ChiEnum> { ChiEnum.Dan, ChiEnum.Than };

                var maoDau = new List<ChiEnum> { ChiEnum.Mao, ChiEnum.Dau };
                var thinTuat = new List<ChiEnum> { ChiEnum.Thin, ChiEnum.Tuat};
                var tyHoi = new List<ChiEnum> { ChiEnum.Ty, ChiEnum.Hoi };

                if (tiNgo.Contains(chi1))
                {
                    lucXung = Constants.DiaChiLucXung.TI_NGO;
                }
                else if (suuMui.Contains(chi1))
                {
                    lucXung = Constants.DiaChiLucXung.SUU_MUI;
                }
                else if (danThan.Contains(chi1))
                {
                    lucXung = Constants.DiaChiLucXung.DAN_THAN;
                }
                else if (maoDau.Contains(chi1))
                {
                    lucXung = Constants.DiaChiLucXung.MAO_DAU;
                }
                else if (thinTuat.Contains(chi1))
                {
                    lucXung = Constants.DiaChiLucXung.THIN_TUAT;
                }
                else if (tyHoi.Contains(chi1))
                {
                    lucXung = Constants.DiaChiLucXung.TY_HOI;
                }

                if (!diaChi1.ThuocTinh.Keys.Contains(thuocTinh))
                {
                    diaChi1.ThuocTinh.Add(thuocTinh, lucXung);
                }

                if (!diaChi2.ThuocTinh.Keys.Contains(thuocTinh))
                {
                    diaChi2.ThuocTinh.Add(thuocTinh, lucXung);
                }

            }
        }
    }

    public class DiaChiLucHai : InteractionLaws
    {
        public DiaChiLucHai(TuTruMap ttm)
        {
            base.Init(ttm);
        }

        public override void SetLaw()
        {
            this.CheckLucHai(ChiEnum.Ti, ChiEnum.Mui);
            this.CheckLucHai(ChiEnum.Suu, ChiEnum.Ngo);
            this.CheckLucHai(ChiEnum.Dan, ChiEnum.Ty);

            this.CheckLucHai(ChiEnum.Mao, ChiEnum.Thin);
            this.CheckLucHai(ChiEnum.Than, ChiEnum.Hoi);
            this.CheckLucHai(ChiEnum.Dau, ChiEnum.Tuat);
        }

        private void CheckLucHai(ChiEnum chi1, ChiEnum chi2)
        {
            var chi1Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi1);
            var chi2Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi2);

            if (chi1Id != -1 && chi2Id != -1)
            {
                var diaChi1 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi1);
                var diaChi2 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi2);

                string thuocTinh = Constants.ThuocTinh.LUC_HAI;

                var lucHai = string.Empty;
                var tiMui = new List<ChiEnum> { ChiEnum.Ti, ChiEnum.Mui };
                var suuNgo = new List<ChiEnum> { ChiEnum.Suu, ChiEnum.Ngo };
                var danTy = new List<ChiEnum> { ChiEnum.Dan, ChiEnum.Ty };

                var maoThin = new List<ChiEnum> { ChiEnum.Mao, ChiEnum.Thin };
                var dauTuat = new List<ChiEnum> { ChiEnum.Dau, ChiEnum.Tuat };
                var thanHoi = new List<ChiEnum> { ChiEnum.Than, ChiEnum.Hoi };

                if (tiMui.Contains(chi1))
                {
                    lucHai = Constants.DiaChiLucHai.TI_MUI;
                }
                else if (suuNgo.Contains(chi1))
                {
                    lucHai = Constants.DiaChiLucHai.SUU_NGO;
                }
                else if (danTy.Contains(chi1))
                {
                    lucHai = Constants.DiaChiLucHai.DAN_TY;
                }
                else if (maoThin.Contains(chi1))
                {
                    lucHai = Constants.DiaChiLucHai.MAO_THIN;
                }
                else if (dauTuat.Contains(chi1))
                {
                    lucHai = Constants.DiaChiLucHai.DAU_TUAT;
                }
                else if (thanHoi.Contains(chi1))
                {
                    lucHai = Constants.DiaChiLucHai.THAN_HOI;
                }

                if (!diaChi1.ThuocTinh.Keys.Contains(thuocTinh))
                {
                    diaChi1.ThuocTinh.Add(thuocTinh, lucHai);
                }

                if (!diaChi2.ThuocTinh.Keys.Contains(thuocTinh))
                {
                    diaChi2.ThuocTinh.Add(thuocTinh, lucHai);
                }

            }
        }
    }

    public class DiaChiTamHoi : InteractionLaws
    {
        public DiaChiTamHoi(TuTruMap ttm)
        {
            base.Init(ttm);
        }

        public override void SetLaw()
        {
            //throw new NotImplementedException();
            this.CheckTamHoi(ChiEnum.Dan, ChiEnum.Mao, ChiEnum.Thin);
            this.CheckTamHoi(ChiEnum.Ty, ChiEnum.Ngo, ChiEnum.Mui);
            this.CheckTamHoi(ChiEnum.Than, ChiEnum.Dau, ChiEnum.Tuat);
            this.CheckTamHoi(ChiEnum.Hoi, ChiEnum.Ti, ChiEnum.Suu);
        }

        private void CheckTamHoi(ChiEnum chi1, ChiEnum chi2, ChiEnum chi3)
        {
            int count = 0;
            DiaChi dc1 = null, dc2 = null, dc3 = null;
            var chi1Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi1);
            var chi2Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi2);
            var chi3Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi3);

            if (chi1Id != -1)
            {
                count++;
                dc1 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi1);
            }

            if (chi2Id != -1)
            {
                count++;
                dc2 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi2);
            }

            if (chi3Id != -1)
            {
                count++;
                dc3 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi3);
            }

            string thuocTinh = string.Empty;
            if (count == 2)
            {
                thuocTinh = Constants.ThuocTinh.BAN_TAM_HOI;
            }

            if (count == 3)
            {
                thuocTinh = Constants.ThuocTinh.TAM_HOI;
            }

            if (dc1 != null)
            {
                this.SetThuocTinh(dc1, thuocTinh);
            }

            if (dc2 != null)
            {
                this.SetThuocTinh(dc2, thuocTinh);
            }

            if (dc3 != null)
            {
                this.SetThuocTinh(dc3, thuocTinh);
            }
        }

        private void SetThuocTinh(DiaChi dc, string thuocTinh)
        {
            var xuan = new List<ChiEnum> { ChiEnum.Dan, ChiEnum.Mao, ChiEnum.Thin };
            var ha = new List<ChiEnum> { ChiEnum.Ty, ChiEnum.Ngo, ChiEnum.Mui };
            var thu = new List<ChiEnum> { ChiEnum.Than, ChiEnum.Dau, ChiEnum.Tuat };
            var dong = new List<ChiEnum> { ChiEnum.Hoi, ChiEnum.Ti, ChiEnum.Suu };

            if (xuan.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Moc);
            }
            else if (ha.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Hoa);
            }
            else if (thu.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Kim);
            }
            else if (dong.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Thuy);
            }
        }
    }

    public class DiaChiTamHop : InteractionLaws
    {
        public DiaChiTamHop(TuTruMap ttm)
        {
            base.Init(ttm);
        }

        public override void SetLaw()
        {
            //throw new NotImplementedException();
            this.CheckTamHop(ChiEnum.Dan, ChiEnum.Ngo, ChiEnum.Tuat);
            this.CheckTamHop(ChiEnum.Ty, ChiEnum.Dau, ChiEnum.Suu);
            this.CheckTamHop(ChiEnum.Than, ChiEnum.Ti, ChiEnum.Thin);
            this.CheckTamHop(ChiEnum.Hoi, ChiEnum.Mao, ChiEnum.Mui);
        }

        private void CheckTamHop(ChiEnum chi1, ChiEnum chi2, ChiEnum chi3)
        {
            int count = 0;
            DiaChi dc1 = null, dc2 = null, dc3 = null;
            var chi1Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi1);
            var chi2Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi2);
            var chi3Id = TatcaTru.FindIndex(u => u.DiaChi.Ten == chi3);

            if (chi1Id != -1)
            {
                count++;
                dc1 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi1);
            }

            if (chi2Id != -1)
            {
                count++;
                dc2 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi2);
            }

            if (chi3Id != -1)
            {
                count++;
                dc3 = TongHopCanChi.MuoiHaiDiaChi.Find(u => u.Ten == chi3);
            }

            string thuocTinh = string.Empty;
            if (count == 2)
            {
                thuocTinh = Constants.ThuocTinh.BAN_TAM_HOP;
            }

            if (count == 3)
            {
                thuocTinh = Constants.ThuocTinh.TAM_HOP;
            }

            if (dc1 != null)
            {
                this.SetThuocTinh(dc1, thuocTinh);
            }

            if (dc2 != null)
            {
                this.SetThuocTinh(dc2, thuocTinh);
            }

            if (dc3 != null)
            {
                this.SetThuocTinh(dc3, thuocTinh);
            }
        }

        private void SetThuocTinh(DiaChi dc, string thuocTinh)
        {
            this.CheckTamHop(ChiEnum.Dan, ChiEnum.Ngo, ChiEnum.Tuat);
            this.CheckTamHop(ChiEnum.Ty, ChiEnum.Dau, ChiEnum.Suu);
            this.CheckTamHop(ChiEnum.Than, ChiEnum.Ti, ChiEnum.Thin);
            this.CheckTamHop(ChiEnum.Hoi, ChiEnum.Mao, ChiEnum.Mui);

            var hoa = new List<ChiEnum> { ChiEnum.Dan, ChiEnum.Ngo, ChiEnum.Tuat };
            var kim = new List<ChiEnum> { ChiEnum.Ty, ChiEnum.Dau, ChiEnum.Suu };
            var thuy = new List<ChiEnum> { ChiEnum.Than, ChiEnum.Ti, ChiEnum.Thin };
            var moc = new List<ChiEnum> { ChiEnum.Hoi, ChiEnum.Mao, ChiEnum.Mui };

            if (hoa.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Hoa);
            }
            else if (kim.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Kim);
            }
            else if (thuy.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Thuy);
            }
            else if (moc.Contains(dc.Ten))
            {
                dc.ThuocTinh.Add(thuocTinh, NguHanhEnum.Moc);
            }
        }
    }
}
