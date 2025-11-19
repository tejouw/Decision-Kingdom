import { Ending, EndingType, ResourceType } from '../models/types.js';

// Tüm bitişler
export const endings: Ending[] = [
  // ============= ÖZEL BİTİŞLER (Yüksek Öncelik) =============
  {
    type: EndingType.CONQUEROR,
    title: 'Fatih Sultan',
    description: 'Güçlü ordunuz ve stratejik zekânızla komşu toprakları fethettiniz. Tarih sizi büyük bir fatih olarak anacak. İmparatorluğunuz genişledi ama halkınız yorgun düştü.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.MILITARY, value: 85 },
      { type: 'flag_set', flag: 'sefer_basladi' },
      { type: 'turn_above', value: 30 }
    ],
    priority: 100
  },
  {
    type: EndingType.BELOVED,
    title: 'Sevilen Sultan',
    description: 'Halkınız sizi çok seviyor! Adaletli yönetiminiz ve cömertliğiniz sayesinde herkesin gönlünde taht kurdunuz. Şarkılar sizin için söyleniyor.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.HAPPINESS, value: 85 },
      { type: 'resource_above', resource: ResourceType.FAITH, value: 70 },
      { type: 'turn_above', value: 25 }
    ],
    priority: 95
  },
  {
    type: EndingType.SCHOLAR,
    title: 'Bilge Sultan',
    description: 'Medreseler, kütüphaneler ve ilim meclisleri kurdunuz. Saltanatınız bir ilim ve irfan çağı olarak anılacak. Alimler diyarınıza akın ediyor.',
    conditions: [
      { type: 'flag_set', flag: 'medrese_acildi' },
      { type: 'flag_set', flag: 'tarih_yaziliyor' },
      { type: 'resource_above', resource: ResourceType.FAITH, value: 75 }
    ],
    priority: 90
  },
  {
    type: EndingType.REVOLUTIONARY,
    title: 'Reformcu Sultan',
    description: 'Cesur reformlarınız toplumu dönüştürdü. Bazıları sizi kahraman, bazıları tehlikeli görüyor. Ama değişim kaçınılmazdı ve siz onu yönettiniz.',
    conditions: [
      { type: 'flag_set', flag: 'reform_yapildi' },
      { type: 'flag_set', flag: 'hosgoru_politikasi' },
      { type: 'turn_above', value: 20 }
    ],
    priority: 85
  },

  // ============= ANA HİKAYE DALI BİTİŞLERİ =============
  {
    type: EndingType.MILITARY,
    title: 'Askeri Diktatörlük',
    description: 'Ordunuz her şeyin üstünde. Generaller sarayda söz sahibi. Düşmanlarınız sizi korkuyla anıyor ama halkınız özgürlüğünü yitirdi.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.MILITARY, value: 75 },
      { type: 'resource_below', resource: ResourceType.HAPPINESS, value: 40 }
    ],
    priority: 70
  },
  {
    type: EndingType.THEOCRATIC,
    title: 'Teokratik Yönetim',
    description: 'Din adamları devletin her kademesinde. İnanç güçlü ama farklı düşünceler bastırıldı. Maneviyat yükseldi, özgür düşünce azaldı.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.FAITH, value: 80 },
      { type: 'flag_set', flag: 'sapkinlik_yasaklandi' }
    ],
    priority: 70
  },
  {
    type: EndingType.MERCHANT,
    title: 'Tüccar Oligarşisi',
    description: 'Ticaret loncaları gerçek güç sahibi oldu. Hazine dolup taşıyor ama zengin ve fakir arasındaki uçurum büyüdü. Parası olan yönetiyor.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.GOLD, value: 80 },
      { type: 'flag_set', flag: 'ipek_yolu_aktif' }
    ],
    priority: 70
  },
  {
    type: EndingType.PEACEFUL,
    title: 'Barışçıl Yönetim',
    description: 'Barış antlaşmaları ve diplomasi sayesinde savaşlardan kaçındınız. Halkınız huzur içinde yaşıyor. Komşularınız size güveniyor.',
    conditions: [
      { type: 'flag_set', flag: 'baris_antlasmasi' },
      { type: 'resource_above', resource: ResourceType.HAPPINESS, value: 60 },
      { type: 'resource_below', resource: ResourceType.MILITARY, value: 50 }
    ],
    priority: 65
  },

  // ============= KAYNAK BAZLI BİTİŞLER =============
  {
    type: EndingType.TYRANNY,
    title: 'Zorba Sultan',
    description: 'Demir yumrukla yönettiniz. Korku hâkim. Kimse size karşı gelemez ama sevgi de yok. Tarihe acımasız biri olarak geçeceksiniz.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.MILITARY, value: 70 },
      { type: 'resource_below', resource: ResourceType.HAPPINESS, value: 30 },
      { type: 'resource_below', resource: ResourceType.FAITH, value: 40 }
    ],
    priority: 60
  },
  {
    type: EndingType.BALANCED,
    title: 'Dengeli Sultan',
    description: 'Tüm kaynakları dengede tuttunuz. Ne çok zengin ne çok fakir, ne çok güçlü ne çok zayıf. İstikrar sağladınız ve halkınız memnun.',
    conditions: [
      { type: 'resource_above', resource: ResourceType.GOLD, value: 40 },
      { type: 'resource_below', resource: ResourceType.GOLD, value: 70 },
      { type: 'resource_above', resource: ResourceType.HAPPINESS, value: 40 },
      { type: 'resource_below', resource: ResourceType.HAPPINESS, value: 70 },
      { type: 'resource_above', resource: ResourceType.MILITARY, value: 40 },
      { type: 'resource_below', resource: ResourceType.MILITARY, value: 70 },
      { type: 'resource_above', resource: ResourceType.FAITH, value: 40 },
      { type: 'resource_below', resource: ResourceType.FAITH, value: 70 }
    ],
    priority: 50
  }
];

// Varsayılan bitiş (hiçbir koşul sağlanmazsa)
export const defaultEnding: Ending = {
  type: EndingType.BALANCED,
  title: 'Sıradan Saltanat',
  description: 'Saltanatınız ne parlak zaferler ne de büyük felaketlerle anıldı. Sıradan bir dönem olarak tarihe geçtiniz.',
  conditions: [],
  priority: 0
};

export default endings;
