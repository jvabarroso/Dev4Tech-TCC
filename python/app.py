from fastapi import FastAPI, File, UploadFile, HTTPException
from fastapi.responses import FileResponse
import os
import uuid
from pathlib import Path
import shutil
import base64

# Importes das bibliotecas de conversão
import comtypes.client
from reportlab.lib.pagesizes import letter
from reportlab.platypus import SimpleDocTemplate, Paragraph
from reportlab.lib.styles import getSampleStyleSheet
import img2pdf
from PIL import Image as PILImage
import pythoncom

app = FastAPI(title="API para conversão de arquivos para PDF", version="1.0.0")

BASE_DIR = r"C:\xampp\htdocs\dev4tech\arquivos"
# Diretórios
Upload_Dir = BASE_DIR
Download_Dir = BASE_DIR
os.makedirs(BASE_DIR, exist_ok=True)

@app.get("/")
async def root():
    return {"message": "Bem-vindo à API de conversão de arquivos para PDF!", "status": "online"}

@app.post("/converter/pdf")
async def converter_para_pdf(file: UploadFile = File(...)):
    arquivo_entrada = None
    arquivo_saida = None
    
    try:
        # Extensões permitidas
        extensoes_permitidas = {'.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx',
                                '.txt', '.jpg', '.jpeg', '.png', '.bmp', '.pdf'}

        extensao = Path(file.filename).suffix.lower()
        if extensao not in extensoes_permitidas:
            raise HTTPException(status_code=400, detail=f"Formato de arquivo não suportado: {extensao}")

        # Caminhos únicos
        file_id = str(uuid.uuid4())
        arquivo_entrada = os.path.join(Upload_Dir, f"{file_id}{extensao}")
        arquivo_saida = os.path.join(Download_Dir, f"{file_id}.pdf")

        # Salvar o arquivo recebido
        with open(arquivo_entrada, "wb") as buffer:
            content = await file.read()
            buffer.write(content)

        print(f"Processando arquivo: {file.filename}, extensão: {extensao}")

        # Processar conforme a extensão
        if extensao == ".pdf":
            arquivo_saida = arquivo_entrada # arquivo_saida = arquivo_entrada
            arquivo_entrada = None   # arquivo_entrada = None
            print ("PDF já está no formato correto. Nenhuma conversão necessária.")     # print("PDF já está no formato correto. Nenhuma conversão necessária."
            print("PDF copiado diretamente")
            if not os.path.exists(arquivo_saida):
                raise HTTPException(status_code=500, detail="Falha ao copiar o PDF")
            print("PDF copiado com sucesso")
        elif extensao in ['.doc', '.docx']:
            await converter_word_para_pdf(arquivo_entrada, arquivo_saida)
            print("Word convertido para PDF")
        elif extensao in ['.xls', '.xlsx']:
            await converter_excel_para_pdf(arquivo_entrada, arquivo_saida)
            print("Excel convertido para PDF")
        elif extensao in ['.ppt', '.pptx']:
            await converter_powerpoint_para_pdf(arquivo_entrada, arquivo_saida)
            print("PowerPoint convertido para PDF")
        elif extensao == '.txt':
            await converter_texto_para_pdf(arquivo_entrada, arquivo_saida)
            print("Texto convertido para PDF")
        elif extensao in ['.jpg', '.jpeg', '.png', '.bmp']:
            await converter_imagem_para_pdf(arquivo_entrada, arquivo_saida)
            print("Imagem convertida para PDF")

        # Verificar se o arquivo de saída foi criado
        if not os.path.exists(arquivo_saida):
            raise HTTPException(status_code=500, detail="Arquivo de saída não foi criado")

        # Limpar arquivo de entrada
        if arquivo_entrada and os.path.exists(arquivo_entrada):
            os.remove(arquivo_entrada)

        return {
            "sucesso": True,
            "mensagem": "Arquivo convertido com sucesso.",
            "arquivo_id": file_id,
            "download_url": f"/download/{file_id}"
        }

    except Exception as e:
        # Limpeza em caso de erro
        if arquivo_entrada and os.path.exists(arquivo_entrada):
            os.remove(arquivo_entrada)
        if arquivo_saida and os.path.exists(arquivo_saida):
            os.remove(arquivo_saida)
            
        print(f"Erro na conversão: {str(e)}")
        raise HTTPException(status_code=500, detail=f"Erro na conversão: {str(e)}")

@app.get("/download/{file_id}")
async def download_pdf(file_id: str):
    arquivo_path = os.path.join(Download_Dir, f"{file_id}.pdf")
    if not os.path.exists(arquivo_path):
        raise HTTPException(status_code=404, detail="Arquivo não encontrado.")

    return FileResponse(
        path=arquivo_path,
        filename=f"{file_id}.pdf",
        media_type='application/pdf'
    )

# ======= FUNÇÕES DE CONVERSÃO =======

async def converter_word_para_pdf(entrada, saida):
    try:
        pythoncom.CoInitialize()
        word = comtypes.client.CreateObject('Word.Application')
        word.Visible = False
        doc = word.Documents.Open(os.path.abspath(entrada))
        doc.SaveAs(os.path.abspath(saida), FileFormat=17)  # 17 = PDF format
        doc.Close()
        word.Quit()
        pythoncom.CoUninitialize()
    except Exception as e:
        raise Exception(f"Erro ao converter Word para PDF: {str(e)}")

async def converter_excel_para_pdf(entrada, saida):
    try:
        pythoncom.CoInitialize()
        excel = comtypes.client.CreateObject('Excel.Application')
        excel.Visible = False
        wb = excel.Workbooks.Open(os.path.abspath(entrada))
        wb.ExportAsFixedFormat(0, os.path.abspath(saida))  # 0 = xlTypePDF
        wb.Close(False)
        excel.Quit()
        pythoncom.CoUninitialize()
    except Exception as e:
        raise Exception(f"Erro ao converter Excel para PDF: {str(e)}")

async def converter_powerpoint_para_pdf(entrada, saida):
    try:
        pythoncom.CoInitialize()
        powerpoint = comtypes.client.CreateObject("Powerpoint.Application")
        powerpoint.Visible = True
        presentation = powerpoint.Presentations.Open(os.path.abspath(entrada), WithWindow=False)
        presentation.SaveAs(os.path.abspath(saida), 32)  # 32 = ppSaveAsPDF
        presentation.Close()
        powerpoint.Quit()
        pythoncom.CoUninitialize()
    except Exception as e:
        raise Exception(f"Erro ao converter PowerPoint para PDF: {str(e)}")

async def converter_texto_para_pdf(entrada, saida):
    try:
        with open(entrada, 'r', encoding='utf-8') as file:
            texto = file.read()

        doc = SimpleDocTemplate(saida, pagesize=letter)
        styles = getSampleStyleSheet()
        elementos = [Paragraph(linha.replace('\n', '<br/>'), styles['Normal']) for linha in texto.split('\n') if linha.strip()]
        doc.build(elementos)
    except Exception as e:
        raise Exception(f"Erro ao converter texto para PDF: {str(e)}")

async def converter_imagem_para_pdf(entrada, saida):
    try:
        # Verificar se a imagem é válida
        with PILImage.open(entrada) as img:
            img.verify()
        
        # Converter usando img2pdf
        with open(entrada, "rb") as image_file:
            image_data = image_file.read()
        with open(saida, "wb") as pdf_file:
            pdf_file.write(img2pdf.convert(image_data))
    except Exception as e:
        raise Exception(f"Erro ao converter imagem: {str(e)}")

if __name__ == "__main__":
    import uvicorn
    print("Iniciando servidor de conversão de arquivos...")
    print("Disponível em: http://0.0.0.0:8000")
    uvicorn.run(app, host="0.0.0.0", port=8000, log_level="info")