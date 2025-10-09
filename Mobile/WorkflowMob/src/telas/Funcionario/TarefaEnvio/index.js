import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, ScrollView, Modal, TextInput, Image } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { LayoutAnimation, UIManager, Platform } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import FlashMessage, { showMessage } from "react-native-flash-message";
import * as DocumentPicker from 'expo-document-picker';

import url from '../../../../services/url';
import api from '../../../../services/api';

export default function TarefaEnvio({ navigation, route }) {
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const tarefa = route.params?.tarefa || {}; 
    const usuario = route.params?.usuario || {}; 
    const filtroAtivo = route.params?.filtroAtivo || {}; 

    console.log("Meus dados: ",usuario.FuncionarioId)

    const [descricao, setDescricao] = useState("");
    const [descricaoExpandida, setDescricaoExpandida] = useState(false);
    const [modalVisivel, setModalVisivel] = useState(false);
    const [problema, setProblema] = useState('');
    const [problemasEnviados, setProblemasEnviados] = useState([]);
    const [tarefaLocal, setTarefaLocal] = useState({ ...tarefa  });
    const [file, setFile] = useState(null);
    const [sucess, setSucess] = useState(false);

    if (Platform.OS === 'android' && UIManager.setLayoutAnimationEnabledExperimental) {
        UIManager.setLayoutAnimationEnabledExperimental(true);
    }

    const alternarDescricao = () => {
        LayoutAnimation.configureNext(LayoutAnimation.Presets.easeInEaseOut);
        setDescricaoExpandida(!descricaoExpandida);
    };

    // Função para formatar datas do banco 
    function formatarData(data) {
    if (!data) return "";
    const partes = data.split("-"); // ["0000","00","00"]
    if (partes.length !== 3) return data;
    return `${partes[2]}/${partes[1]}/${partes[0]}`;
    }

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
            message: 'Nenhuma arquivo selecionada.',
            description: 'Por favor, selecione um arquivo primeiro.',
            floating: true,
            statusBarHeight: 70,
            type: "danger",
            duration: 2000,             
            });
            return false;
        };

        let filename = file.name;
        let type = file.mimeType || "application/octet-stream";

        let formData = new FormData();
        formData.append("file", { uri: file.uri, name: filename, type });

        try {
            const response = await fetch(`${url}/dev4tech//upload_arquivos.php`, {
                method: "POST",
                body: formData
            });

            const text = await response.text();
            let resJson;
            console.log("Resposta do servidor:", text);
            
            try {
                resJson = JSON.parse(text);
            } catch (e) {
                console.error("Erro ao converter JSON:", e);
            }

            if (response.ok && resJson.success) {        
                showMessage({
                    message: 'Sucesso.',
                    description: 'Tarefa enviada com sucesso!',
                    floating: true,
                    statusBarHeight: 70,
                    type: "success",
                    duration: 2000,             
            });
            return resJson.file;
            
            } else {
                showMessage({
                message: 'Erro.',
                description: resJson.message || "Falha ao enviar Tarefa.",
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 2000,             
                });
                return false;
            }
        } catch (error) {
            console.error(error);
            showMessage({
                message: 'Erro.',
                description: "Ocorreu um erro ao tentar enviar a tarefa.",
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 2000,             
            });
            return false;
            }
        }

    //Relata o problema
    async function relatoproblema() {      
        try {
            const res = await api.post('dev4tech/relatoproblema.php', {
                id_tarefa : tarefa.id_tarefa, 
                id_equipe : tarefa.id_equipe, 
                descricao : problema, 
                id_empresa: usuario.id_empresa,
            });

            if (res.data.sucesso) {
                setTarefaLocal(res.data.tarefa); // atualiza selproblema
                setProblemasEnviados(res.data.problemas);
                setProblema('');
                showMessage({
                    message: "Relatado com sucesso",
                    type: "success",
                });
            } else {
                showMessage({
                    message: res.data.mensagem,
                    type: "warning",
                });
            }

        } catch (error) {
            console.log(error);
            showMessage({
                message: "Erro ao relatar problema",
                type: "danger",
            });
        }
    };

    //Carrega os problemas
    useEffect(() => {
        carregarProblemasExistentes();
    }, []);

    async function carregarProblemasExistentes() {
        try {
            if (tarefa.id_tarefa) {

                const res = await api.get('dev4tech/verificarproblemas.php', {
                    params: { id_tarefa: tarefa.id_tarefa }
                });
                if (res.data.sucesso) {
                    setProblemasEnviados(res.data.problemas);
                    setTarefaLocal(prev => ({...prev, selproblema: res.data.problemas.length > 0}));
                }
            }
        } catch (error) {
            console.log("Erro ao carregar problemas:", error);
        }
    }

    //Carrega a tarefa, caso estiver concluida
    useEffect(() => {
        carregartarefa();
    }, []);

    async function carregartarefa() {
        try {
            if (tarefa.id_tarefa) {

                const res = await api.get('dev4tech/carregartarefa.php', {
                    params: { id_tarefa: tarefa.id_tarefa }
                });
                if (res.data.sucesso) {
                    const tarefaConcluida = res.data.tarefa;
                    setDescricao(tarefaConcluida.descricao || '');
                    if (tarefaConcluida.nome_arquivo) {
                        setFile({ name: tarefaConcluida.nome_arquivo, uri: '', mimeType: '' });
                    }
                }
            }
        } catch (error) {
            console.log("Erro ao carregar Tarefas:", error);
        }
    }

    //Envia a mensagem
    const enviarProblema = () => {
        if(problema.trim()) {
            relatoproblema();
        }
    };


    //Desfaz Tarefas
    async function desfazerTarefas() {
        try {
            const res = await api.get(`dev4tech/desfazertarefas.php`, {
                params: { id_tarefa: tarefa.id_tarefa }
        });
        console.log(res.data);

        if (res.data && res.data.success) {
            showMessage({
                message: 'Desfeita a entrega.',
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
            });
        } else {
            showMessage({
                message: 'Ocorre um erro ao Desfazer entrega.',
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
            });
        }

        }
        catch (error) {
        console.log("Erro ao Desfazer Tarefa:", error);
        showMessage({
            message: "Erro ao Desfazer Tarefa:",
            description:"Erro de conexão com o servidor",
            floating: true,
            statusBarHeight: 70,
            type: "warning",
            duration: 2000,             
        });
        }
    }

    //Entrega a Tarefa
    async function entrega() {      
        const arquivo = await uploadFile();
        if (!arquivo) return;  

        try {
            const res = await api.post('dev4tech/enviotarefas.php', {
                id_tarefa : tarefa.id_tarefa,
                id_equipe : tarefa.id_equipe,
                descricao : descricao, 
                nome_arquivo: arquivo,
                FuncionarioId: usuario.FuncionarioId
            });

            if (res.data.sucesso === false) {

            showMessage({
                message: "Erro ao entregar Tarefa",
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
                message: "Entregado com Sucesso",
                description: "Tarefa entregado",
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
            });         

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

    return (
        <View style={styles.container}>
            <ScrollView 
                style={styles.scrollView}
                contentContainerStyle={styles.containerConteudo}
                showsVerticalScrollIndicator={false}
            >
                <View style={styles.nav}>
                    <TouchableOpacity 
                        style={styles.botaodevoltar}
                        onPress={() => navigation.goBack()}
                    >
                        <Ionicons name="arrow-back" size={25} color={theme.text} />
                    </TouchableOpacity>
                    <Image 
                        style={styles.titulo}
                        source={theme.logo} >
                    </Image>
                    <View style={styles.espacoHeader} />
                </View>

                <View style={styles.areadetalhes}>
                    <Text style={styles.titulotarefa}>{tarefa.nomeTarefa}</Text>

                    <Text style={styles.datadeenvio}>Postado em {formatarData(tarefa.data_criacao)}</Text>
                    
                    {tarefaLocal.selproblema && (
                        <View style={[styles.textoproblem, styles.problem]}>
                            <Ionicons name="warning-outline" size={24} color="red" style={{ marginRight: 8 }}/>
                            <Text style={styles.textoproblem}>Problema Relatado</Text>
                        </View>
                    )}

                    <View style={styles.linha}>
                        <View style={styles.coluna}>
                            <Text style={styles.subtitulos}>PRAZO DE ENTREGA</Text>
                            <Text style={styles.datas}>{formatarData(tarefa.data_entrega)}</Text>
                        </View>

                        <View style={styles.colunaEquipe}>
                            <Text style={styles.subtitulos}>EQUIPE</Text>
                            <View style={styles.cargos}>
                                <Text style={styles.textoCargo}>{tarefa.nome_equipe}</Text>
                            </View>
                        </View>
                    </View>

                    <View style={styles.linha2}>
                        <Text style={styles.titulodescricao}>DESCRIÇÃO DA TAREFA</Text>
                        <Text style={styles.descricao2}>
                            {descricaoExpandida 
                                ? (tarefa.instrucoes || "Sem instruções disponíveis")
                                : tarefa.instrucoes
                                    ? `${tarefa.instrucoes.slice(0, 100)}${tarefa.instrucoes.length > 100 ? '...' : ''}`
                                    : "Sem instruções disponíveis"}
                        </Text>
                        {tarefa.instrucoes.length > 100 && (
                            <TouchableOpacity onPress={alternarDescricao}>
                                <Text style={styles.textodescr}>
                                    {descricaoExpandida ? 'Ver menos' : 'Ver mais'}
                                </Text>
                            </TouchableOpacity>
                        )}
                    </View>

                    <View style={styles.linha2}>
                        <Text style={styles.subtitulos}>MEU TRABALHO</Text>

                        {/* So para não ficar confuso, esse mostra a descrição */}
                        {filtroAtivo === "concluido" ? (  
                            <View>
                                <Text style={styles.texto}>Comentário</Text>
                                <View style={[styles.inputinstrucoes, { padding: 8, minHeight: 100 }]}>
                                    <Text style={{ color: theme.text }}>{descricao}</Text>
                                </View>
                            </View>
                        ):  
                            <View>
                                <Text style={styles.texto}>Comentário</Text>
                                <TextInput
                                    style={styles.inputinstrucoes}
                                    multiline
                                    numberOfLines={7}
                                    placeholder="Digite um comentário..."
                                    placeholderTextColor={theme.text}
                                    onChangeText={setDescricao}
                                />
                            </View>
                        }

                        {/* Esse para Anexar Arquivo */}
                        {filtroAtivo === "concluido" ? (                        
                            <View style={styles.botaomostrar}>
                                <Text style={styles.textoadd}>{file?.name || ''}</Text>
                            </View>

                        ):
                            <TouchableOpacity 
                                style={styles.botaomostrar}
                                onPress={pickDocument}
                            >
                                <Text style={styles.textoadd}>{file ? file.name : "Anexar um arquivo"}</Text>
                            </TouchableOpacity>
                        }

                        {/* Esse para relatar problema*/}
                        {filtroAtivo !== "concluido" && (
                        <TouchableOpacity
                            style={styles.botaomostrar}
                            onPress={() => setModalVisivel(true)}
                        >
                            <Text style={styles.textoproblem}>Relatar problema</Text>
                        </TouchableOpacity>
                        )}
                    </View>
                    
                    {/* Esse para relatar Enviar ou desfazer entrega*/}
                    {filtroAtivo === "concluido" ? (                        
                        <TouchableOpacity
                            style={[styles.botaoenviar, { backgroundColor: "#FF4444" }]}
                            onPress={desfazerTarefas}
                        >
                            <Text style={styles.textoenvio}>Desfazer Entrega</Text>
                        </TouchableOpacity>

                    ):
                        <TouchableOpacity 
                            style={styles.botaoenviar}
                            onPress={entrega}
                        >
                            <Text style={styles.textoenvio}>Enviar</Text>
                        </TouchableOpacity>
                    }
                </View>
            </ScrollView> 

            <Modal
                animationType="slide"
                transparent={false}
                visible={modalVisivel}
                onRequestClose={() => setModalVisivel(false)}
            >   
                <View style={styles.modalContainer}>
                    <View style={styles.modalContent}>
                        <View style={styles.nav2}>
                            <TouchableOpacity 
                                style={styles.botaodevoltar2}
                                onPress={() => setModalVisivel(false)}
                            >
                                <Ionicons name="arrow-back" size={28} color="black" />
                            </TouchableOpacity>
                            <Text style={styles.titulo2}>WORKFLOW</Text>
                            <View style={styles.espacoHeader} />
                        </View>
                        <View style={styles.modalMainContent}>
                            <View style={styles.containermensagem}>
                                <View style={styles.mensagem}>
                                    <Text style={styles.modeltexto}>Qual é o problema?</Text>
                                </View>

                                {problemasEnviados.map((item, index) => (
                                    <View key={index} style={[styles.mensagem, styles.mensagemEnviada, {marginTop: 10}]}>
                                        <Text style={styles.modeltexto}>{item}</Text>
                                    </View>
                                    ))}
                                </View>

                            <View style={styles.imagemfundo}>
                                <Ionicons name="warning-outline" size={200} color="#999999" />                     
                            </View>

                            <View style={styles.espacoInput} />

                            <View style={styles.containerinput}>
                                <TextInput
                                    style={styles.textInput}
                                    multiline
                                    numberOfLines={4}
                                    placeholder="Reporte seu problema"
                                    placeholderTextColor={theme.text}
                                    value={problema}
                                    onChangeText={setProblema}
                                    underlineColorAndroid="transparent"
                                />
                                <TouchableOpacity 
                                    style={styles.botaoenviar}
                                    onPress={enviarProblema}
                                >
                                    <Ionicons name="paper-plane-outline" size={24} color="#1C58F2" style={styles.iconSobreposto} /> 
                                    
                                </TouchableOpacity>
                            </View>
                        </View>
                    </View>
                <FlashMessage position="top" />
                </View>
            </Modal>
        </View>
    );
}