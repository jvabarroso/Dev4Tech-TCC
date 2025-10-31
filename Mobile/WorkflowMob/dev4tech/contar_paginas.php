<?php
include_once('conexao.php');

function contarPaginasPDFComPdftools($caminhoArquivo) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado'];
    }

    $pdftkAvailable = shell_exec('which pdftk') !== null;
    
    if ($pdftkAvailable) {
        $command = "pdftk \"$caminhoArquivo\" dump_data | grep NumberOfPages | awk '{print $2}'";
        $paginas = shell_exec($command);
        
        if ($paginas !== null) {
            $paginas = intval(trim($paginas));
            return [
                'success' => true, 
                'total_paginas' => $paginas,
                'nome_arquivo' => basename($caminhoArquivo),
                'metodo' => 'pdftk'
            ];
        }
    }
    return contarPaginasPDF($caminhoArquivo);
}

function contarPaginasPDF($caminhoArquivo) {
    if (!file_exists($caminhoArquivo)) {
        return ['success' => false, 'message' => 'Arquivo não encontrado'];
    }

    $mime = mime_content_type($caminhoArquivo);
    if ($mime !== 'application/pdf') {
        return ['success' => false, 'message' => 'O arquivo não é um PDF válido'];
    }

    try {
        $handle = fopen($caminhoArquivo, "r");
        if (!$handle) {
            return ['success' => false, 'message' => 'Não foi possível abrir o arquivo'];
        }

        $paginas = 0;
        $conteudo = fread($handle, filesize($caminhoArquivo));
        fclose($handle);

        if (preg_match_all('/\/Count\s+(\d+)/', $conteudo, $matches)) {
            $paginas = max($matches[1]);
        }
        
        if ($paginas === 0) {
            preg_match_all('/\/Type\s*\/Page[^s]/', $conteudo, $matches);
            $paginas = count($matches[0]);
        }

        if ($paginas === 0) {
            preg_match_all('/\/Parent\s*\d+\s*0\s*R.*?\/Count\s*(\d+)/s', $conteudo, $matches);
            if (!empty($matches[1])) {
                $paginas = max($matches[1]);
            }
        }

        if ($paginas > 0) {
            return [
                'success' => true, 
                'total_paginas' => $paginas,
                'nome_arquivo' => basename($caminhoArquivo),
                'tamanho_arquivo' => filesize($caminhoArquivo)
            ];
        } else {
            return ['success' => false, 'message' => 'Não foi possível determinar o número de páginas'];
        }

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro ao processar PDF: ' . $e->getMessage()];
    }
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);
    $caminhoArquivo = $input['caminho_arquivo'] ?? '';
    
    if (empty($caminhoArquivo)) {
        echo json_encode(['success' => false, 'message' => 'Caminho do arquivo não fornecido']);
        exit;
    }

    $resultado = contarPaginasPDF($caminhoArquivo);
    echo json_encode($resultado);
    exit;
}
?>