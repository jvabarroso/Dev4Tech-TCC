import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, Modal, TextInput } from 'react-native';
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
            description: 'Por favor, selecione ou tire uma foto primeiro.',
            floating: true,
            statusBarHeight: 70,
            type: "danger",
            duration: 2000,             
            });
            return false;
        };

        let filename = file.split('/').pop();
        let match = /\.(\w+)$/.exec(filename);
        let type = match ? `file/${match[1]}` : `file`;

        let formData = new FormData();
        formData.append("file", { uri: file, name: filename,type });

        try {
            const res = await fetch(`${url}/upload_arquivos.php`, {
                method: "POST",
                body: formData
        });

        const text = await response.text();
        let resJson;
            
        try {
            resJson = JSON.parse(text);
        } catch (e) {
            console.error("Erro ao converter JSON:", e);
        }

        if (response.ok && resJson.success) {        
            showMessage({
                message: 'Sucesso.',
                description: 'Imagem enviada com sucesso!',
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
        });
        return resJson.file;
            
        } else {
            showMessage({
            message: 'Erro.',
            description: resJson.message || "Falha ao enviar imagem.",
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
            description: "Ocorreu um erro ao tentar enviar a imagem.",
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
            const res = await api.post('dev4tec/relatoproblema.php', {
                id_tarefa : tarefa.id_tarefa, 
                id_equipe : tarefa.id_equipe, 
                descricao : problema, 
                id_empresa: usuario.id_empresa,
            });

            if (res.data.sucesso === false) {

            showMessage({
                message: "Erro ao Relatar o problema",
                description: res.data.mensagem,
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 3000,                    
            });            
            return;
            }

            setSucess(true);
                showMessage({
                message: "Relatado com sucesso com Sucesso",
                description: "Relato Registrado",
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
            });         

            } 
        catch (error) {
            console.log("Erro no envio do relato:", error.message);
            if (error.response) {
                console.log("RESPOSTA DO SERVIDOR:", error.response.data);
            }
            if (error.request) {
                console.log("SEM RESPOSTA, REQUEST:", error.request);
            }
            setSucess(false);
            showMessage({
                message: "Tente novamente.",
                description: res.data.mensagem,
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 3000,                    
            });  
        }
        
    }   

    //Envia a mensagem
    const enviarProblema = () => {
        relatoproblema()
        if (problema.trim()) {
                setProblemasEnviados([...problemasEnviados, problema]);
                setProblema('');
                setTarefaLocal({ ...tarefaLocal, selproblema: true }); //ajustado :D
        }
    };
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
                        <Ionicons name="arrow-back" size={24} color={theme.text} />
                    </TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                    <View style={styles.espacoHeader} />
                </View>

                <View style={styles.areadetalhes}>
                    <Text style={styles.titulotarefa}>{tarefa.nomeTarefa}</Text>

                    {tarefaLocal.selproblema && (
                    <Ionicons name="warning-outline" size={24} color="red" style={{ marginTop: 5 }} />
                    )}

                    <Text style={styles.datadeenvio}>Postado em {tarefa.data_criacao}</Text>
                    
                    {tarefaLocal.selproblema && (
                        <View style={[styles.textoproblem, styles.problem]}>
                            <Text style={styles.textoproblem}>Problema Relatado</Text>
                        </View>
                    )}

                    <View style={styles.linha}>
                        <View style={styles.coluna}>
                            <Text style={styles.subtitulos}>PRAZO DE ENTREGA</Text>
                            <Text style={styles.datas}>{tarefa.data_entrega}</Text>
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
                        <TouchableOpacity 
                            style={styles.botaomostrar}
                            onPress={pickDocument}
                        >
                            <Text style={styles.textoadd}>Anexar um arquivo {file ? `|| ${file.name}` : ""}</Text>
                        </TouchableOpacity>
                        
                        {!tarefaLocal.selproblema && (
                            <TouchableOpacity
                                style={styles.botaomostrar}
                                onPress={() => setModalVisivel(true)}
                            >
                                <Text style={styles.textoproblem}>Relatar problema</Text>
                            </TouchableOpacity>
                        )}

                    </View>

                    <TouchableOpacity style={styles.botaoenviar}>
                        <Text style={styles.textoenvio}>Enviar</Text>
                    </TouchableOpacity>
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
                                    onPress={relatoproblema}
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