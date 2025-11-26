<?php
include_once('conexao.php');

function dividirPDFReal($caminhoArquivo, $idTarefa) {
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

        // MÉTODO 1: Usar Python com PyPDF2 para DIVIDIR REALMENTE
        $pythonAvailable = shell_exec('where python') !== null;
        
        if ($pythonAvailable) {
            return dividirPDFComPython($caminhoArquivo, $idTarefa, $pastaDestino);
        }

        // MÉTODO 2: Se Python não estiver disponível, usar fallback
        return dividirPDFFallback($caminhoArquivo, $idTarefa, $pastaDestino);

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro ao processar PDF: ' . $e->getMessage()];
    }
}

function dividirPDFComPython($caminhoArquivo, $idTarefa, $pastaDestino) {
    try {
        // Criar script Python para DIVIDIR o PDF
        $pythonScript = "
import PyPDF2
import sys
import os

try:
    # Abrir o PDF original
    with open('" . str_replace('\\', '\\\\', $caminhoArquivo) . "', 'rb') as file:
        pdf = PyPDF2.PdfReader(file)
        total_pages = len(pdf.pages)
        
        # Para CADA página, criar um PDF individual
        for page_num in range(total_pages):
            # Criar novo PDF
            writer = PyPDF2.PdfWriter()
            # Adicionar apenas UMA página
            writer.add_page(pdf.pages[page_num])
            
            # Nome do arquivo de saída
            output_file = os.path.join('" . str_replace('\\', '\\\\', $pastaDestino) . "', f'tarefa_{$idTarefa}_pagina_{page_num + 1}.pdf')
            
            # Salvar o PDF individual
            with open(output_file, 'wb') as output:
                writer.write(output)
            
            print(f'Página {page_num + 1} criada: {output_file}')
        
        print(f'TOTAL_PAGINAS:{total_pages}')
        
except Exception as e:
    print(f'ERRO:{str(e)}')
";

        // Salvar e executar o script Python
        file_put_contents('dividir_pdf.py', $pythonScript);
        $output = shell_exec('python dividir_pdf.py 2>&1');
        unlink('dividir_pdf.py'); // Limpar arquivo temporário

        // Analisar a saída do Python
        if (strpos($output, 'TOTAL_PAGINAS:') !== false) {
            // Extrair número total de páginas
            preg_match('/TOTAL_PAGINAS:(\d+)/', $output, $matches);
            $totalPaginas = intval($matches[1]);
            
            // Verificar se os arquivos foram criados
            $arquivosCriados = 0;
            for ($i = 1; $i <= $totalPaginas; $i++) {
                $arquivo = $pastaDestino . 'tarefa_' . $idTarefa . '_pagina_' . $i . '.pdf';
                if (file_exists($arquivo) && filesize($arquivo) > 0) {
                    $arquivosCriados++;
                }
            }

            if ($arquivosCriados === $totalPaginas) {
                return [
                    'success' => true, 
                    'total_paginas' => $totalPaginas,
                    'paginas_criadas' => $arquivosCriados,
                    'mensagem' => 'PDF dividido com SUCESSO em ' . $totalPaginas . ' páginas individuais',
                    'modo' => 'divisao_real_python',
                    'observacao' => 'Cada arquivo contém APENAS UMA página do PDF original'
                ];
            }
        }

        // Se chegou aqui, houve erro no Python
        if (strpos($output, 'ERRO:') !== false) {
            preg_match('/ERRO:(.+)/', $output, $errorMatches);
            $erro = $errorMatches[1] ?? 'Erro desconhecido no Python';
            return ['success' => false, 'message' => 'Erro no Python: ' . $erro];
        }

        return ['success' => false, 'message' => 'Falha na divisão com Python. Saída: ' . $output];

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro no processo Python: ' . $e->getMessage()];
    }
}

function dividirPDFFallback($caminhoArquivo, $idTarefa, $pastaDestino) {
    // MÉTODO FALLBACK: contar páginas e criar cópias (seu método atual)
    try {
        $handle = fopen($caminhoArquivo, "rb");
        $conteudo = fread($handle, min(filesize($caminhoArquivo), 100000));
        fclose($handle);

        // Detectar número de páginas
        $totalPaginas = 1;
        if (preg_match_all('/\/Count\s+(\d+)/', $conteudo, $matches)) {
            $totalPaginas = max(array_map('intval', $matches[1]));
        }

        // Criar cópias (modo fallback)
        for ($i = 1; $i <= $totalPaginas; $i++) {
            $nomeArquivo = 'tarefa_' . $idTarefa . '_pagina_' . $i . '.pdf';
            $arquivoSaida = $pastaDestino . $nomeArquivo;
            copy($caminhoArquivo, $arquivoSaida);
        }

        return [
            'success' => true, 
            'total_paginas' => $totalPaginas,
            'mensagem' => 'PDF processado em modo fallback com ' . $totalPaginas . ' páginas',
            'modo' => 'simulacao_fallback',
            'observacao' => 'Cada arquivo contém o PDF completo (não dividido)'
        ];

    } catch (Exception $e) {
        return ['success' => false, 'message' => 'Erro no fallback: ' . $e->getMessage()];
    }
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    header('Content-Type: application/json');
    
    $input = json_decode(file_get_contents('php://input'), true);
    $caminhoArquivo = $input['caminho_arquivo'] ?? '';
    $idTarefa = $input['id_tarefa'] ?? '';
    
    if (empty($caminhoArquivo) || empty($idTarefa)) {
        echo json_encode(['success' => false, 'message' => 'Dados insuficientes']);
        exit;
    }

    $resultado = dividirPDFReal($caminhoArquivo, $idTarefa);
    echo json_encode($resultado);
    exit;
}
?>