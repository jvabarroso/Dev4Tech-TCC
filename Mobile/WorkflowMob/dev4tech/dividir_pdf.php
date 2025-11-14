<?php
include_once('conexao.php');

function contarPaginasPDF($caminhoArquivo) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado'];
    }

    try {
        $handle = fopen($caminhoArquivo, "r");
        if (!$handle) {
            return ['success' => false, 'message' => 'Não foi possível abrir o arquivo'];
        }

        $conteudo = fread($handle, filesize($caminhoArquivo));
        fclose($handle);

        // Método 1: Procurar por /Count
        if (preg_match_all('/\/Count\s+(\d+)/', $conteudo, $matches)) {
            $paginas = max($matches[1]);
            if ($paginas > 0) {
                return ['success' => true, 'total_paginas' => $paginas];
            }
        }

        // Método 2: Procurar por /Type/Page
        if (preg_match_all('/\/Type\s*\/Page[^s]/', $conteudo, $matches)) {
            $paginas = count($matches[0]);
            if ($paginas > 0) {
                return ['success' => true, 'total_paginas' => $paginas];
            }
        }

        // Método 3: Procurar por /Kids
        if (preg_match_all('/\/Kids\s*\[([^\]]+)\]/', $conteudo, $matches)) {
            $kids = preg_split('/\s+/', $matches[1][0]);
            $paginas = count(array_filter($kids, function($item) {
                return strpos($item, '0 R') !== false;
            }));
            if ($paginas > 0) {
                return ['success' => true, 'total_paginas' => $paginas];
            }
        }

        return ['success' => false, 'message' => 'Não foi possível determinar o número de páginas'];

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro ao processar PDF: ' . $e->getMessage()];
    }
}

function dividirPDFComCopias($caminhoArquivo, $idTarefa, $totalPaginas) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado'];
    }

    try {
        $pastaDestino = 'C:/xampp/htdocs/dev4tech/arquivos/' . $idTarefa . '/';

        // Criar pasta se não existir
        if (!file_exists($pastaDestino)) {
            if (!mkdir($pastaDestino, 0777, true)) {
                return ['success' => false, 'message' => 'Não foi possível criar a pasta de destino'];
            }
        }

        // Ler o conteúdo do PDF original
        $conteudoOriginal = file_get_contents($caminhoArquivo);
        
        // Para cada página, criar um arquivo "simulado" com o PDF completo
        // (Esta é uma solução temporária - o usuário verá o PDF completo mas navegará pelas páginas)
        for ($i = 1; $i <= $totalPaginas; $i++) {
            $arquivoSaida = $pastaDestino . 'pagina_' . $i . '.pdf';
            
            // Simplesmente copiar o arquivo original para cada "página"
            // Em uma implementação real, você precisaria de uma biblioteca PDF
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

    // Primeiro contar páginas
    $resultadoContagem = contarPaginasPDF($caminhoArquivo);
    if (!$resultadoContagem['success']) {
        echo json_encode($resultadoContagem);
        exit;
    }

    // Depois "dividir" (na verdade criar cópias para simulação)
    $resultado = dividirPDFComCopias($caminhoArquivo, $idTarefa, $resultadoContagem['total_paginas']);
    echo json_encode($resultado);
    exit;
}
?>