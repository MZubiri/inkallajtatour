import { Injectable, signal, computed } from '@angular/core';

export type Language = 'es' | 'en';

export interface TourTranslation {
  title: string;
  shortDescription: string;
  duration: string;
  location: string;
}

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private readonly STORAGE_KEY = 'inkallajta_lang';

  // Current language signal
  currentLang = signal<Language>(this.getInitialLang());

  // Dictionary
  private readonly translations: Record<Language, Record<string, string>> = {
    es: {
      // Nav
      'nav.home': 'Inicio',
      'nav.tours': 'Tours',
      'nav.whyUs': 'Por qué elegirnos',
      'nav.testimonials': 'Testimonios',
      'nav.contact': 'Contacto',
      'nav.admin': 'Panel Admin',
      'nav.currency': 'Moneda',
      'nav.lang': 'Idioma',
      'nav.bookNow': 'Reservar',

      // Hero
      'hero.badge': 'Agencia Oficial de Turismo Cusco & Machu Picchu',
      'hero.title1': 'Vive la Magia de',
      'hero.title2': 'Machu Picchu & Cusco',
      'hero.subtitle': 'Operador local directo. Salidas 100% garantizadas, guías certificados y experiencias inolvidables en los Andes peruanos.',
      'hero.searchPlaceholder': '¿Qué destino buscas? (Ej: Machu Picchu, Humantay, Vinicunca...)',
      'hero.btnSearch': 'Buscar Tours',
      'hero.filterAll': 'Todos los Tours',

      // Stats
      'stats.travelers': 'Viajeros Felices',
      'stats.experience': 'Años de Experiencia',
      'stats.satisfaction': 'Satisfacción',
      'stats.support': 'Soporte 24/7',

      // Tour categories
      'cat.all': 'Todos los Tours',
      'cat.machuPicchu': 'Tours Machu Picchu',
      'cat.dayTours': 'Day Tours',
      'cat.treks': 'Treks de Aventura',
      'cat.peru': 'Tours del Perú',

      // Tour cards
      'tour.from': 'Desde',
      'tour.perPerson': '/ persona',
      'tour.duration': 'Duración',
      'tour.difficulty': 'Dificultad',
      'tour.diffEasy': 'Fácil',
      'tour.diffModerate': 'Moderado',
      'tour.diffHard': 'Exigente',
      'tour.maxGroup': 'Grupo máx',
      'tour.people': 'personas',
      'tour.viewDetail': 'Ver Itinerario',
      'tour.bookNow': 'Reservar',
      'tour.featured': 'Destacado',
      'tour.noResults': 'No se encontraron tours con esa búsqueda.',
      'tour.clearFilters': 'Restablecer filtros',

      // Why choose us
      'why.badge': '¿Por qué Inkallajta?',
      'why.title': 'Tu viaje soñado en las mejores manos',
      'why.subtitle': 'Somos una agencia cusqueña apasionada por compartir la cultura inca con respeto, seguridad y excelencia.',
      'why.f1Title': 'Operadores Locales Directos',
      'why.f1Desc': 'Sin intermediarios. Trato directo, precios justos y asistencia personalizada antes, durante y después de tu viaje.',
      'why.f2Title': 'Guías Oficiales Bilingües',
      'why.f2Desc': 'Profesionales apasionados nacidos en Cusco, expertos en arqueología, historia inca y primeros auxilios.',
      'why.f3Title': 'Salidas 100% Garantizadas',
      'why.f3Desc': 'Tu reserva está asegurada con confirmación inmediata, gestión formal de boletos y trenes autorizados.',
      'why.f4Title': 'Turismo Responsable',
      'why.f4Desc': 'Comprometidos con el pago justo a porteadores, respeto al medio ambiente y apoyo a comunidades locales.',

      // Testimonials
      'test.badge': 'Opiniones Reales',
      'test.title': 'Lo que dicen quienes ya viajaron con nosotros',
      'test.subtitle': 'Más de 10,000 aventureros de todo el mundo confían en Inkallajta Tour para sus vacaciones en Perú.',
      'test.verified': 'Viajero Verificado',

      // Contact
      'contact.badge': 'Ponte en Contacto',
      'contact.title': '¿Planificando tu viaje a Cusco?',
      'contact.subtitle': 'Escríbenos y un especialista en viajes diseñará tu itinerario ideal sin compromiso.',
      'contact.infoTitle': 'Información de Contacto',
      'contact.addressLabel': 'Dirección',
      'contact.address': 'Calle Plateros 358, Centro Histórico, Cusco - Perú',
      'contact.phoneLabel': 'WhatsApp & Llamadas',
      'contact.emailLabel': 'Correo Electrónico',
      'contact.hoursLabel': 'Horario de Atención',
      'contact.hours': 'Lunes a Domingo: 7:00 AM - 9:00 PM',
      'contact.formName': 'Tu Nombre Completo',
      'contact.formEmail': 'Tu Correo Electrónico',
      'contact.formPhone': 'WhatsApp / Teléfono',
      'contact.formSubject': 'Asunto',
      'contact.formMessage': 'Cuéntanos sobre tus fechas y planes...',
      'contact.btnSend': 'Enviar Mensaje',
      'contact.sending': 'Enviando...',
      'contact.success': '¡Mensaje enviado con éxito! Te responderemos muy pronto.',

      // Booking Modal
      'book.modalTitle': 'Reservar Tour',
      'book.step1': '1. Datos del Viajero',
      'book.name': 'Nombre Completo',
      'book.email': 'Correo Electrónico',
      'book.phone': 'WhatsApp / Teléfono',
      'book.country': 'País de Origen',
      'book.date': 'Fecha de Viaje',
      'book.people': 'Número de Pasajeros',
      'book.special': 'Solicitudes Especiales (dietas, recojo, etc.)',
      'book.summary': 'Resumen de Reserva',
      'book.pricePerPerson': 'Precio por persona:',
      'book.subtotal': 'Subtotal estimado:',
      'book.btnConfirm': 'Confirmar Pre-Reserva',
      'book.submitting': 'Procesando reserva...',
      'book.successTitle': '¡Pre-Reserva Confirmada!',
      'book.successDesc': 'Hemos registrado tu solicitud con código #{id}. Te contactaremos a la brevedad para coordinar boletos y detalles.',
      'book.btnWhatsApp': 'Confirmar por WhatsApp Ahora',
      'book.close': 'Cerrar',

      // Tour Detail
      'detail.back': 'Volver a Tours',
      'detail.overview': 'Descripción del Tour',
      'detail.itinerary': 'Itinerario Detallado',
      'detail.includes': 'Qué Incluye',
      'detail.excludes': 'Qué No Incluye',
      'detail.gallery': 'Galería de Fotos',
      'detail.sidebarTitle': 'Reserva tu Aventura',
      'detail.btnReserve': 'Reservar Ahora',
      'detail.inquireWa': 'Consultar por WhatsApp',
      'detail.pricePerPerson': 'por persona',
      'detail.maxGroup': 'Grupo Máximo',
      'detail.difficulty': 'Dificultad',
      'detail.duration': 'Duración',
      'detail.location': 'Ubicación',

      // Footer
      'footer.desc': 'Agencia de viajes y turismo receptivo en Cusco, Perú. Especialistas en Machu Picchu, Valle Sagrado, caminatas de aventura y circuitos por todo el país.',
      'footer.quickLinks': 'Enlaces Rápidos',
      'footer.topTours': 'Tours Populares',
      'footer.contact': 'Contacto',
      'footer.rights': 'Todos los derechos reservados. Diseñado para ofrecer la mejor experiencia turística del Perú.'
    },

    en: {
      // Nav
      'nav.home': 'Home',
      'nav.tours': 'Tours',
      'nav.whyUs': 'Why Us',
      'nav.testimonials': 'Reviews',
      'nav.contact': 'Contact',
      'nav.admin': 'Admin Portal',
      'nav.currency': 'Currency',
      'nav.lang': 'Language',
      'nav.bookNow': 'Book Now',

      // Hero
      'hero.badge': 'Official Licensed Tour Agency Cusco & Machu Picchu',
      'hero.title1': 'Experience the Magic of',
      'hero.title2': 'Machu Picchu & Cusco',
      'hero.subtitle': 'Direct local tour operator. 100% guaranteed departures, certified bilingual guides, and unforgettable Andean adventures.',
      'hero.searchPlaceholder': 'Where do you want to go? (e.g., Machu Picchu, Humantay, Rainbow Mountain...)',
      'hero.btnSearch': 'Search Tours',
      'hero.filterAll': 'All Tours',

      // Stats
      'stats.travelers': 'Happy Travelers',
      'stats.experience': 'Years Experience',
      'stats.satisfaction': 'Satisfaction',
      'stats.support': '24/7 Support',

      // Tour categories
      'cat.all': 'All Tours',
      'cat.machuPicchu': 'Machu Picchu Tours',
      'cat.dayTours': 'Day Tours',
      'cat.treks': 'Adventure Treks',
      'cat.peru': 'Peru Circuits',

      // Tour cards
      'tour.from': 'From',
      'tour.perPerson': '/ person',
      'tour.duration': 'Duration',
      'tour.difficulty': 'Difficulty',
      'tour.diffEasy': 'Easy',
      'tour.diffModerate': 'Moderate',
      'tour.diffHard': 'Challenging',
      'tour.maxGroup': 'Max group',
      'tour.people': 'travelers',
      'tour.viewDetail': 'View Itinerary',
      'tour.bookNow': 'Book Now',
      'tour.featured': 'Featured',
      'tour.noResults': 'No tours found matching your search.',
      'tour.clearFilters': 'Reset filters',

      // Why choose us
      'why.badge': 'Why Inkallajta?',
      'why.title': 'Your Dream Journey in Expert Hands',
      'why.subtitle': 'We are native Cusqueños dedicated to sharing Inca history with authenticity, warmth, and uncompromised safety.',
      'why.f1Title': 'Direct Local Operators',
      'why.f1Desc': 'No middlemen. Fair transparent prices, direct communication, and personalized care from inquiry to departure.',
      'why.f2Title': 'Certified Bilingual Guides',
      'why.f2Desc': 'Born in Cusco, licensed professionals passionate about archaeology, Inca lore, and wilderness safety.',
      'why.f3Title': '100% Guaranteed Departures',
      'why.f3Desc': 'Your trip is protected with immediate booking confirmation, official entrance tickets, and authorized train permits.',
      'why.f4Title': 'Sustainable Travel',
      'why.f4Desc': 'Strict commitment to fair porter wages, leave-no-trace ecological practices, and uplifting native Andean communities.',

      // Testimonials
      'test.badge': 'Verified Reviews',
      'test.title': 'What Our Travelers Say About Us',
      'test.subtitle': 'Over 10,000 adventurers worldwide have trusted Inkallajta Tour for their Peruvian vacation.',
      'test.verified': 'Verified Traveler',

      // Contact
      'contact.badge': 'Get In Touch',
      'contact.title': 'Planning Your Journey to Cusco?',
      'contact.subtitle': 'Message our travel experts for personalized itinerary recommendations and advice.',
      'contact.infoTitle': 'Contact Information',
      'contact.addressLabel': 'Address',
      'contact.address': '358 Plateros St, Historic Center, Cusco - Peru',
      'contact.phoneLabel': 'WhatsApp & Phone',
      'contact.emailLabel': 'Email Address',
      'contact.hoursLabel': 'Business Hours',
      'contact.hours': 'Monday to Sunday: 7:00 AM - 9:00 PM',
      'contact.formName': 'Full Name',
      'contact.formEmail': 'Email Address',
      'contact.formPhone': 'WhatsApp / Phone',
      'contact.formSubject': 'Subject',
      'contact.formMessage': 'Tell us about your dates and travel ideas...',
      'contact.btnSend': 'Send Message',
      'contact.sending': 'Sending...',
      'contact.success': 'Message sent successfully! We will get back to you shortly.',

      // Booking Modal
      'book.modalTitle': 'Book Your Tour',
      'book.step1': '1. Traveler Details',
      'book.name': 'Full Name',
      'book.email': 'Email Address',
      'book.phone': 'WhatsApp / Phone',
      'book.country': 'Country of Origin',
      'book.date': 'Travel Date',
      'book.people': 'Number of Travelers',
      'book.special': 'Special Requests (dietary, hotel pickup, etc.)',
      'book.summary': 'Booking Summary',
      'book.pricePerPerson': 'Price per person:',
      'book.subtotal': 'Estimated Subtotal:',
      'book.btnConfirm': 'Confirm Reservation',
      'book.submitting': 'Processing booking...',
      'book.successTitle': 'Reservation Confirmed!',
      'book.successDesc': 'We received your reservation with code #{id}. Our team will contact you shortly to coordinate ticket logistics.',
      'book.btnWhatsApp': 'Confirm on WhatsApp Now',
      'book.close': 'Close',

      // Tour Detail
      'detail.back': 'Back to Tours',
      'detail.overview': 'Tour Overview',
      'detail.itinerary': 'Detailed Itinerary',
      'detail.includes': 'What is Included',
      'detail.excludes': 'What is Not Included',
      'detail.gallery': 'Photo Gallery',
      'detail.sidebarTitle': 'Book Your Adventure',
      'detail.btnReserve': 'Book Now',
      'detail.inquireWa': 'Inquire on WhatsApp',
      'detail.pricePerPerson': 'per person',
      'detail.maxGroup': 'Max Group Size',
      'detail.difficulty': 'Difficulty',
      'detail.duration': 'Duration',
      'detail.location': 'Location',

      // Footer
      'footer.desc': 'Inbound travel agency and tour operator based in Cusco, Peru. Specialists in Machu Picchu, Sacred Valley, adventure treks, and custom circuits across Peru.',
      'footer.quickLinks': 'Quick Links',
      'footer.topTours': 'Popular Tours',
      'footer.contact': 'Contact Us',
      'footer.rights': 'All rights reserved. Designed to deliver the finest travel experience in Peru.'
    }
  };

  // English tour translations by tour ID
  private readonly tourTranslationsEn: Record<number, TourTranslation> = {
    1: {
      title: 'Machu Picchu 1 Day Tour',
      shortDescription: 'Visit the Wonder of the World on a full-day tour from Cusco with an official bilingual guide and scenic train ride.',
      duration: '1 Day (Full day)',
      location: 'Cusco - Machu Picchu'
    },
    2: {
      title: 'Sacred Valley Connection to Machu Picchu 2D/1N',
      shortDescription: 'Combine the Sacred Valley of the Incas with mystical Machu Picchu in an unforgettable 2-day experience.',
      duration: '2 Days / 1 Night',
      location: 'Sacred Valley - Machu Picchu'
    },
    3: {
      title: 'Cusco & Machu Picchu 3D/2N',
      shortDescription: 'Complete package: Imperial City Tour, Sacred Valley, and Machu Picchu in 3 days of cultural adventure.',
      duration: '3 Days / 2 Nights',
      location: 'Cusco - Sacred Valley - Machu Picchu'
    },
    4: {
      title: 'Cusco Historic City Tour',
      shortDescription: 'Tour the most sacred historical monuments of the imperial city of Cusco in half a day.',
      duration: 'Half Day (5 hours)',
      location: 'Cusco'
    },
    5: {
      title: 'Rainbow Mountain (Vinicunca) Trek',
      shortDescription: 'Hike to the breathtaking Rainbow Mountain at over 5,000 meters above sea level. A natural wonder.',
      duration: '1 Day (14 hours)',
      location: 'Cusipata - Vinicunca'
    },
    6: {
      title: 'Humantay Glacial Lake Hike',
      shortDescription: 'Discover the turquoise alpine lake at the foot of Mount Humantay, an awe-inspiring Andean glacier paradise.',
      duration: '1 Day (13 hours)',
      location: 'Mollepata - Humantay Lake'
    },
    7: {
      title: 'Maras Salt Mines & Moray Concentric Terraces',
      shortDescription: 'Marvel at thousands of ancient artisanal salt pools in Maras and the circular Inca agricultural laboratory of Moray.',
      duration: 'Half Day (5 hours)',
      location: 'Maras - Moray'
    },
    8: {
      title: 'Salkantay Trek 5D/4N to Machu Picchu',
      shortDescription: 'The epic Inca Trail alternative. Cross the Salkantay pass at 4,630m and hike down into the subtropical jungle to Machu Picchu.',
      duration: '5 Days / 4 Nights',
      location: 'Mollepata - Salkantay - Machu Picchu'
    },
    9: {
      title: 'Cusco South Valley: Tipón & Andahuaylillas',
      shortDescription: 'Discover Tipón hydraulic engineering, the pre-Inca Wari city of Piquillacta, and the Sistine Chapel of the Americas.',
      duration: 'Half Day (5 hours)',
      location: 'Tipón - Piquillacta - Andahuaylillas'
    },
    10: {
      title: 'Cusco, Machu Picchu & Lake Titicaca 5D/4N',
      shortDescription: 'The ultimate southern Peru package: imperial Cusco, Machu Picchu, and the floating totora reed islands of Lake Titicaca.',
      duration: '5 Days / 4 Nights',
      location: 'Cusco - Machu Picchu - Puno'
    }
  };

  private getInitialLang(): Language {
    try {
      const saved = localStorage.getItem(this.STORAGE_KEY) as Language;
      if (saved === 'es' || saved === 'en') return saved;
      // Browser language check
      if (typeof navigator !== 'undefined' && navigator.language?.startsWith('en')) {
        return 'en';
      }
    } catch {
      // ignore
    }
    return 'es';
  }

  setLanguage(lang: Language): void {
    this.currentLang.set(lang);
    try {
      localStorage.setItem(this.STORAGE_KEY, lang);
    } catch {
      // ignore
    }
  }

  toggleLanguage(): void {
    this.setLanguage(this.currentLang() === 'es' ? 'en' : 'es');
  }

  // Get translated string by key
  t(key: string): string {
    const lang = this.currentLang();
    return this.translations[lang]?.[key] || this.translations['es']?.[key] || key;
  }

  // Translate tour data for display
  getTourTitle(tourId: number, defaultTitle: string): string {
    if (this.currentLang() === 'en') {
      return this.tourTranslationsEn[tourId]?.title || defaultTitle;
    }
    return defaultTitle;
  }

  getTourShortDesc(tourId: number, defaultDesc: string): string {
    if (this.currentLang() === 'en') {
      return this.tourTranslationsEn[tourId]?.shortDescription || defaultDesc;
    }
    return defaultDesc;
  }

  getTourDuration(tourId: number, defaultDuration: string): string {
    if (this.currentLang() === 'en') {
      return this.tourTranslationsEn[tourId]?.duration || defaultDuration;
    }
    return defaultDuration;
  }

  getTourLocation(tourId: number, defaultLocation: string): string {
    if (this.currentLang() === 'en') {
      return this.tourTranslationsEn[tourId]?.location || defaultLocation;
    }
    return defaultLocation;
  }

  getDifficultyLabel(diff: string): string {
    if (diff === 'easy') return this.t('tour.diffEasy');
    if (diff === 'moderate') return this.t('tour.diffModerate');
    return this.t('tour.diffHard');
  }
}
