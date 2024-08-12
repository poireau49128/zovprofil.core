const swiper_main = new Swiper('.swiper-main', {
    direction: 'horizontal',
    rewind: true,
    autoplay: {
        delay: 5000,
        disableOnInteraction: false,
    },
    scrollbar: {
        el: '.swiper-scrollbar',
        draggable: true,
    },
});


const swiper_latest = new Swiper('.swiper-latest', {
    slidesPerView: 1,
    spaceBetween: 30,
    grabCursor: true,
    rewind: true,
    autoplay: {
        delay: 5000,
        disableOnInteraction: false,
    },
    scrollbar: {
        el: '.swiper-scrollbar',
        draggable: true,
    },
    breakpoints: {
        500: {
            slidesPerView: 2,
            spaceBetween: 20,
        },
        768: {
            slidesPerView: 3,
            spaceBetween: 20,
        },
        1024: {
            slidesPerView: 4,
            spaceBetween: 20,
        },
        1366: {
            slidesPerView: 5,
            spaceBetween: 50,
        },
    }
});