// Smooth scrolling para links de navegação
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Adicionar classe ativa ao header quando scrollar
window.addEventListener('scroll', () => {
    const header = document.querySelector('.header');
    if (window.scrollY > 100) {
        header.style.background = 'rgba(255, 255, 255, 0.95)';
        header.style.backdropFilter = 'blur(10px)';
    } else {
        header.style.background = 'white';
        header.style.backdropFilter = 'none';
    }
});

// Funcionalidade dos botões
document.querySelectorAll('.btn-primary').forEach(button => {
    button.addEventListener('click', function() {
        if (this.textContent.includes('Baixar')) {
            alert('Redirecionando para download...');
            // Aqui você pode adicionar o link real do download
        } else if (this.textContent.includes('Cadastrar')) {
            alert('Redirecionando para cadastro...');
            // Aqui você pode adicionar o link real do cadastro
        }
    });
});

document.querySelectorAll('.btn-secondary').forEach(button => {
    button.addEventListener('click', function() {
        if (this.textContent.includes('Demonstração')) {
            alert('Abrindo demonstração...');
            // Aqui você pode adicionar o link real da demonstração
        } else if (this.textContent.includes('Entrar')) {
            alert('Redirecionando para login...');
            // Aqui você pode adicionar o link real do login
        }
    });
});

// Animação de entrada dos elementos
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observar elementos para animação
document.addEventListener('DOMContentLoaded', () => {
    const animatedElements = document.querySelectorAll('.hero-content, .hero-image, .about-header, .feature-card, .benefits-header, .benefit-card, .benefits-graphic, .team-header, .team-image, .value-card, .cta-content, .footer-contact');
    animatedElements.forEach(el => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(30px)';
        el.style.transition = 'opacity 0.8s ease, transform 0.8s ease';
        observer.observe(el);
    });
});

// Adicionar efeito de hover nos botões
document.querySelectorAll('button').forEach(button => {
    button.addEventListener('mouseenter', function() {
        this.style.transform = 'translateY(-2px)';
    });
    
    button.addEventListener('mouseleave', function() {
        this.style.transform = 'translateY(0)';
    });
    
    // Funcionalidade específica para o botão "Sobre o Projeto"
    if (this.textContent.includes('Sobre o Projeto')) {
        this.addEventListener('click', function() {
            // Scroll suave para a seção de funcionalidades
            const featuresSection = document.querySelector('.features-grid');
            if (featuresSection) {
                featuresSection.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    }
    
    // Funcionalidade específica para o botão "Benefícios"
    if (this.textContent.includes('Benefícios')) {
        this.addEventListener('click', function() {
            // Scroll suave para a seção de benefícios
            const benefitsSection = document.querySelector('.benefits-cards');
            if (benefitsSection) {
                benefitsSection.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    }
    
    // Funcionalidade específica para o botão "Nossa Equipe"
    if (this.textContent.includes('Nossa Equipe')) {
        this.addEventListener('click', function() {
            // Scroll suave para a seção da equipe
            const teamSection = document.querySelector('.team-content');
            if (teamSection) {
                teamSection.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    }
    
    // Funcionalidade específica para os botões CTA
    if (this.textContent.includes('Download Gratuito')) {
        this.addEventListener('click', function() {
            alert('Iniciando download do WORKFLOW...');
            // Aqui você pode adicionar o link real do download
        });
    }
    
    if (this.classList.contains('btn-cta-secondary')) {
        this.addEventListener('click', function() {
            alert('Funcionalidade em desenvolvimento...');
            // Aqui você pode adicionar a funcionalidade desejada
        });
    }
});
