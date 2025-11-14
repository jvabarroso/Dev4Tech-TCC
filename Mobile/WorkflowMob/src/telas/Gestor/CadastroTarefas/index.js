import React, { useState, useEffect} from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Dropdown } from 'react-native-element-dropdown';
import { showMessage } from "react-native-flash-message";
import * as DocumentPicker from 'expo-document-picker';
import DateTimePickerModal from "react-native-modal-datetime-picker";
import { Ionicons } from '@expo/vector-icons';

import url from '../../../../services/url';
import api from '../../../../services/api';
import fonts from "../../../styles/fonts";

export default function CadastroTarefas({ route, navigation }){
    const { theme } = useTheme();
    const styles = getStyles(theme);
    const fastApiUrl = "http://10.239.0.124:8000/converter/pdf";

    const usuario = route.params?.usuario;
    const [sucess, setSucess] = useState(false); 

    const [nomeTarefa, setNomeTarefa] = useState('');
    const [instrucoes, setInstrucoes] = useState('');
    const [data, setData] = useState('');
    const [isDatePickerVisible, setDatePickerVisibility] = useState(false);

    const [equipe, setEquipe] = useState(null);
    const [equipeselecionada, setEquipeSelecionada] = useState(null);
    const [dificuldadeselecionada, setDificuldadeSelecionada] = useState(null);
    
    const [dados, setDados] = useState([]);
    const [file, setFile] = useState(null);

    const showDatePicker = () => setDatePickerVisibility(true);
    const hideDatePicker = () => setDatePickerVisibility(false);

    const dificuldadeOptions = [
    { label: "Fácil", value: "facil" },
    { label: "Médio", value: "medio" },
    { label: "Difícil", value: "dificil" },
    ];

    // Função chamada quando o usuário escolhe a data
    const handleConfirm = (date) => {
        const hoje = new Date();
        if (date <= hoje) {
        showMessage({
            message: "Data inválida",
            description: "Escolha uma data futura para entrega.",
            type: "warning",
            floating: true,
            duration: 2000,   
            statusBarHeight: 70,
        });
        hideDatePicker();
        return;
        }  

        const formatada = date.toLocaleDateString("pt-BR");
        setData(formatada);
        hideDatePicker();
    };


    //Formata a Data para o Banco
    function formatarDataParaBanco(data) {
      if (!data) return '';
      
      // Se já está no formato do banco, retorna direto
      if (/^\d{4}-\d{2}-\d{2}$/.test(data)) return data;
      
      // Converte de DD/MM/AAAA para AAAA-MM-DD
      const partes = data.split('/');
      if (partes.length === 3) {
        return `${partes[2]}-${partes[1]}-${partes[0]}`;
      }
      return data;
    }

    //Formata a Data
    function formatarDataInput(text) {
      let data = text.replace(/\D/g, '');
      
      if (data.length > 2) data = `${data.slice(0,2)}/${data.slice(2)}`;
      if (data.length > 5) data = `${data.slice(0,5)}/${data.slice(5,9)}`;
      
      return data.slice(0,10);
    }
    
    //Lista Equipes
    async function listarDados() {
    if (!usuario?.AdminId) {
            console.log("ID do usuário não disponível");
            return;
    }
    
    try {
        const res = await api.get(`dev4tech/equipeadm.php`, {
        params: {
            id_administrador: usuario.AdminId // Use o ID do usuário logado
        }
        });

        if (res.data.success) {
        setDados(res.data.result || []);
        } else {
        console.log("Erro na API:", res.data.message);
        setDados([]);
        }
    }
    catch (error) {
        console.log("Erro ao listar equipes:", error);
    }
    }

    useEffect(() => {
        listarDados();
    }, [usuario?.AdminId]);

    //Seleciona o Arquivo
    async function pickDocument() {
        try {
        const result = await DocumentPicker.getDocumentAsync({
            type: "*/*", // aceita qualquer tipo de arquivo
            copyToCacheDirectory: true,
        });

        if (result.canceled) {
            console.log("Usuário cancelou a seleção");
            return;
        }

        console.log(result);
        setFile(result.assets[0]); // Pega o arquivo selecionado
        } catch (err) {
        console.error("Erro ao selecionar documento:", err);
        }
    }

    //Envia o Arquivo
    async function uploadFile() {
        if (!file) {
            showMessage({
                message: 'Nenhum arquivo selecionado.',
                description: 'Por favor, selecione um arquivo.',
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
            });
            return false;
        }

        let filename = file.name;
        let type = file.mimeType || "application/octet-stream";

        let formData = new FormData();
        formData.append("file", { uri: file.uri, name: filename, type });

        try {
            const response = await fetch(fastApiUrl, {
                method: "POST",
                body: formData,
                headers: {
                    "Accept": "application/json",
                },
            });

            const data = await response.json();
            console.log("Resposta do FastAPI:", data);

            if (response.ok && data.sucesso) {
                // Retorna a URL de download do PDF gerado
                showMessage({
                    message: 'Arquivo convertido com sucesso.',
                    description: 'PDF disponível para download.',
                    floating: true,
                    statusBarHeight: 70,
                    type: "success",
                    duration: 2000,             
                });
                return data.arquivo_id; 
            } else {
                showMessage({
                    message: 'Erro na conversão',
                    description: data.mensagem || "Falha ao converter arquivo.",
                    floating: true,
                    statusBarHeight: 70,
                    type: "warning",
                    duration: 2000,             
                });
                return false;
            }
        } catch (error) {
            console.error("Erro ao enviar arquivo:", error);
            showMessage({
                message: 'Erro',
                description: "Não foi possível enviar o arquivo.",
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
            });
            return false;
        }
    }

    useEffect(() => {
    }, []);
    //Cadastra a Tarefa
    async function cadastra() {      
        const arquivoPDF = await uploadFile();
        if (!arquivoPDF) return;
            try {
                const res = await api.post('dev4tech/cadastrotarefas.php', {
                    nomeTarefa : nomeTarefa,
                    instrucoes : instrucoes,
                    id_equipe : equipe, 
                    data_entrega: formatarDataParaBanco(data),
                    nome_arquivo: arquivoPDF,
                    dificuldade: dificuldadeselecionada,
                    id_empresa: usuario.id_empresa,
                });

                if (res.data.sucesso === false) {

                showMessage({
                    message: "Erro ao cadastrar Tarefa",
                    description: res.data.mensagem,
                    floating: true,
                    statusBarHeight: 70,
                    type: "warning",
                    duration: 3000,                    
                });      
                console.log(res.data.mensagem)       
                return;
                }

                setSucess(true);
                    showMessage({
                    message: "Cadastrado com Sucesso",
                    description: "Tarefa cadastrada",
                    floating: true,
                    statusBarHeight: 70,
                    type: "success",
                    duration: 2000,             
                });         
                limparCampos()
                } 
            catch (error) {
                console.log("Erro no Envio:", error.message);
                if (error.response) {
                    console.log("Resposta do Servidor:", error.response.data);
                }
                if (error.request) {
                    console.log("Sem resposta, request:", error.request);
                }
                setSucess(false);
                showMessage({
                    message: "Alguma coisa deu errado, tente novamente.",
                    description: res.data.mensagem,
                    floating: true,
                    statusBarHeight: 70,
                    type: "warning",
                    duration: 3000,                    
                });  
            }
    }   
    function limparCampos(){
        setNomeTarefa('');
        setInstrucoes('');
        setData('');
        setEquipe(null);         
        setEquipeSelecionada(null); 
        setDificuldadeSelecionada(null);
        setFile(null);
    }

    return(
        <View style={styles.container}>
            <ScrollView contentContainerStyle={styles.scrollContent}>
                <Text style={styles.titulo}>Adicionar uma tarefa</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Tarefa</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Desenvolver o App"
                        placeholderTextColor={theme.text3}
                        value={nomeTarefa}  
                        onChangeText={(text) => setNomeTarefa(text)}
                    />
                    <Text style={styles.texto}>Instruções</Text>
                    <TextInput
                        style={styles.inputinstrucoes}
                        multiline
                        numberOfLines={7}
                        placeholder="Alteração nos valores contratuais."
                        placeholderTextColor={theme.text3}
                        textAlignVertical="top"
                        value={instrucoes}
                        onChangeText={(text) => setInstrucoes(text)}
                        maxLength={250}
                    />
                    <Text style={styles.texto}>Equipes</Text>
                    <Dropdown
                        style={[styles.input,{borderColor: '#D6D3D1',borderWidth: 1,borderRadius: 6,}]}
                        data={dados}
                        labelField="nome_equipe" 
                        valueField="id_equipe"
                        placeholder={equipeselecionada || "Selecione uma equipe"}
                        placeholderStyle={{ color: theme.text3, fontSize: 14 }}
                        selectedTextStyle={{ color: theme.text, fontSize: 14 }}
                        value={equipe}
                        onChange={item => {
                            setEquipe(item.id_equipe);
                            setEquipeSelecionada(item.nome_equipe);
                        }}
                        containerStyle={{
                            backgroundColor: theme.inputBackground,
                        }}
                        itemTextStyle={{
                            color: theme.text,
                            fontSize: 14,
                            fontFamily: fonts.text,
                        }}
                        selectedStyle={{
                            color: theme.text,
                            fontSize: 14,
                            fontFamily: fonts.text,
                        }}
                        activeColor={theme.inputBackground} 
                    />
                    <Text style={styles.texto}>Dificuldade</Text>
                    <Dropdown
                        style={[styles.input,{borderColor: '#D6D3D1',borderWidth: 1,borderRadius: 6,}]}
                        data={dificuldadeOptions}
                        labelField="label" 
                        valueField="value"
                        placeholder={dificuldadeselecionada || "Selecione uma Dificuldade"}
                        placeholderStyle={{ color: theme.text3, fontSize: 14 }}
                        selectedTextStyle={{ color: theme.text, fontSize: 14 }}
                        value={dificuldadeselecionada}
                        onChange={item => {
                            setDificuldadeSelecionada(item.label);
                        }}
                        containerStyle={{
                            backgroundColor: theme.inputBackground,
                        }}
                        itemContainerStyle={{
                            backgroundColor: theme.inputBackground,
                        }}
                        itemTextStyle={{
                            color: theme.text,
                            fontSize: 14,
                            fontFamily: fonts.text,
                        }}
                        selectedStyle={{
                            color: theme.text,
                            fontSize: 14,
                            fontFamily: fonts.text,
                        }}
                        activeColor={theme.inputBackground} 
                    />
                    <Text style={styles.texto}>Data de entrega</Text>

                    <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                        <TextInput
                            style={[styles.input, { marginRight: 5, width: '77%' }]}
                            placeholder="00/00/0000"
                            placeholderTextColor={theme.text3}
                            keyboardType="numeric"
                            value={data}
                            onChangeText={(text) => setData(formatarDataInput(text))}
                            maxLength={10}
                        />
                        <TouchableOpacity
                            onPress={showDatePicker}
                            style={ styles.databotao}
                        >
                            <Ionicons name="calendar-outline" size={22} color={theme.text} />
                        </TouchableOpacity>
                    </View>

                    <DateTimePickerModal
                        isVisible={isDatePickerVisible}
                        mode="date"
                        display="default"
                        themeVariant={theme.mode === 'dark' ? 'dark' : 'light'}
                        onConfirm={handleConfirm}
                        onCancel={hideDatePicker}
                        minimumDate={new Date()}
                        locale="pt-BR"
                        confirmTextIOS="Confirmar"
                        cancelTextIOS="Cancelar"
                    />

                    <TouchableOpacity
                        style={[styles.botaoanexo,styles.linha]}
                        onPress={pickDocument}
                    >
                        <View style={styles.textosanexo}>
                            <Ionicons name="document-text-outline" size={18} color="#3288D7" />
                            <Text style={styles.textoanexo}>{file ? file.name : "Anexar um arquivo"}</Text>  
                        </View>
                    </TouchableOpacity>

                    <TouchableOpacity 
                        style={styles.botaocriar}
                        onPress={cadastra}
                    >
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
