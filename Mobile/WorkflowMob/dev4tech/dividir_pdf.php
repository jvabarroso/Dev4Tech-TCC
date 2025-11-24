<?php
include_once('conexao.php');

// Incluir a biblioteca FPDI
require_once 'vendor/autoload.php';

use setasign\Fpdi\Fpdi;

function contarPaginasPDF($caminhoArquivo) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado: ' . $caminhoArquivo];
    }

    try {
        // Usar FPDI para contar páginas de forma mais confiável
        $pdf = new Fpdi();
        $pageCount = $pdf->setSourceFile($caminhoArquivo);
        
        if ($pageCount > 0) {
            return ['success' => true, 'total_paginas' => $pageCount];
        } else {
            return ['success' => false, 'message' => 'PDF não contém páginas ou está corrompido'];
        }

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro ao contar páginas: ' . $e->getMessage()];
    }
}

function dividirPDFReal($caminhoArquivo, $idTarefa) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado: ' . $caminhoArquivo];
    }

    try {
        $pastaDestino = 'C:/xampp/htdocs/dev4tech/arquivos/';

        // Verificar se a pasta arquivos existe
        if (!file_exists($pastaDestino)) {
            if (!mkdir($pastaDestino, 0777, true)) {
                return ['success' => false, 'message' => 'Não foi possível criar a pasta de destino'];
            }
        }

        // Criar instância do FPDI
        $pdf = new Fpdi();
        
        // Obter o número total de páginas
        $pageCount = $pdf->setSourceFile($caminhoArquivo);
        
        if ($pageCount === 0) {
            return ['success' => false, 'message' => 'PDF não contém páginas'];
        }

        $paginasCriadas = 0;

        // Para cada página, criar um PDF individual
        for ($pageNo = 1; $pageNo <= $pageCount; $pageNo++) {
            // Criar novo PDF para esta página
            $pdfIndividual = new Fpdi();
            
            // Adicionar página
            $pdfIndividual->AddPage();
            
            // Importar a página específica do PDF original
            $templateId = $pdfIndividual->setSourceFile($caminhoArquivo);
            $pageId = $pdfIndividual->importPage($pageNo);
            
            // Usar a página importada
            $pdfIndividual->useTemplate($pageId);
            
            // Nome do arquivo
            $nomeArquivo = 'tarefa_' . $idTarefa . '_pagina_' . $pageNo . '.pdf';
            $arquivoSaida = $pastaDestino . $nomeArquivo;
            
            // Salvar o PDF individual
            $pdfIndividual->Output($arquivoSaida, 'F');
            
            // Verificar se o arquivo foi criado
            if (file_exists($arquivoSaida) && filesize($arquivoSaida) > 0) {
                $paginasCriadas++;
            } else {
                return ['success' => false, 'message' => "Falha ao criar página $pageNo"];
            }
        }

        return [
            'success' => true, 
            'total_paginas' => $pageCount,
            'paginas_criadas' => $paginasCriadas,
            'pasta_paginas' => $pastaDestino,
            'mensagem' => 'PDF dividido com sucesso em ' . $pageCount . ' páginas individuais',
            'modo' => 'divisao_real'
        ];

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro ao dividir PDF: ' . $e->getMessage()];
    }
}

// Função alternativa se FPDI não funcionar
function dividirPDFComCopias($caminhoArquivo, $idTarefa, $totalPaginas) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado: ' . $caminhoArquivo];
    }

    try {
        $pastaDestino = 'C:/xampp/htdocs/dev4tech/arquivos/';

        if (!file_exists($pastaDestino)) {
            if (!mkdir($pastaDestino, 0777, true)) {
                return ['success' => false, 'message' => 'Não foi possível criar a pasta de destino'];
            }
        }

        // Para cada página, criar uma cópia (modo simulação)
        for ($i = 1; $i <= $totalPaginas; $i++) {
            $nomeArquivo = 'tarefa_' . $idTarefa . '_pagina_' . $i . '.pdf';
            $arquivoSaida = $pastaDestino . $nomeArquivo;
            
            if (!copy($caminhoArquivo, $arquivoSaida)) {
                return ['success' => false, 'message' => "Falha ao criar página $i"];
            }
        }

        return [
            'success' => true, 
            'total_paginas' => $totalPaginas,
            'pasta_paginas' => $pastaDestino,
            'mensagem' => 'PDF preparado com ' . $totalPaginas . ' páginas (modo simulação)',
            'modo' => 'simulacao'
        ];

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro ao processar PDF: ' . $e->getMessage()];
    }
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);
    $caminhoArquivo = $input['caminho_arquivo'] ?? '';
    $idTarefa = $input['id_tarefa'] ?? '';
    
    if (empty($caminhoArquivo) || empty($idTarefa)) {
        echo json_encode(['success' => false, 'message' => 'Dados insuficientes']);
        exit;
    }

    // Primeiro tentar divisão real com FPDI
    $resultado = dividirPDFReal($caminhoArquivo, $idTarefa);
    
    // Se FPDI falhar, usar modo simulação
    if (!$resultado['success']) {
        // Contar páginas primeiro
        $resultadoContagem = contarPaginasPDF($caminhoArquivo);
        if ($resultadoContagem['success']) {
            $resultado = dividirPDFComCopias($caminhoArquivo, $idTarefa, $resultadoContagem['total_paginas']);
        } else {
            $resultado = $resultadoContagem;
        }
    }
    
    echo json_encode($resultado);
    exit;
}
?>