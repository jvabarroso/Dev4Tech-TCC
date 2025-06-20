import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, Modal, TextInput } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { LayoutAnimation, UIManager, Platform } from 'react-native';
import { Ionicons } from '@expo/vector-icons';

export default function TarefaEnvio({ navigation, route }) {
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const { tarefas } = route.params;
    const [descricaoExpandida, setDescricaoExpandida] = useState(false);
    const [modalVisivel, setModalVisivel] = useState(false);
    const [problema, setProblema] = useState('');
    const [problemasEnviados, setProblemasEnviados] = useState([]);
    const [tarefaLocal, setTarefaLocal] = useState({ ...tarefas });


    if (Platform.OS === 'android' && UIManager.setLayoutAnimationEnabledExperimental) {
        UIManager.setLayoutAnimationEnabledExperimental(true);
    }

    const alternarDescricao = () => {
        LayoutAnimation.configureNext(LayoutAnimation.Presets.easeInEaseOut);
        setDescricaoExpandida(!descricaoExpandida);
    };

    const enviarProblema = () => {
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
                        onPress={() => navigation.navigate('Home', { screen: 'Tarefas' })}
                    >
                        <Ionicons name="arrow-back" size={24} color={theme.text} />
                    </TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                    <View style={styles.espacoHeader} />
                </View>

                <View style={styles.areadetalhes}>
                    <Image source={tarefas.imagem} style={styles.imagem} />
                    <Text style={styles.titulotarefa}>{tarefas.titulo}</Text>

                    {tarefaLocal.selproblema && (
                    <Ionicons name="warning-outline" size={24} color="red" style={{ marginTop: 5 }} />
                    )}

                    <Text style={styles.datadeenvio}>Postado em {tarefas.datadeenvio}</Text>
                    
                    {tarefaLocal.selproblema && (
                        <View style={[styles.textoproblem, styles.problem]}>
                            <Text style={styles.textoproblem}>Problema Relatado</Text>
                        </View>
                    )}

                    <View style={styles.linha}>
                        <View style={styles.coluna}>
                            <Text style={styles.subtitulos}>PRAZO DE ENTREGA</Text>
                            <Text style={styles.datas}>{tarefas.datadeentrega}</Text>
                        </View>

                        <View style={styles.colunaEquipe}>
                            <Text style={styles.subtitulos}>EQUIPE</Text>
                            <View style={styles.cargos}>
                                <Text style={styles.textoCargo}>{tarefas.cargo}</Text>
                            </View>
                        </View>
                    </View>

                    <View style={styles.linha2}>
                        <Text style={styles.titulodescricao}>DESCRIÇÃO DA TAREFA</Text>
                        <Text style={styles.descricao2}>
                            {descricaoExpandida 
                                ? tarefas.descricao 
                                : `${tarefas.descricao.slice(0, 100)}${tarefas.descricao.length > 100 ? '...' : ''}`
                            }
                        </Text>
                        {tarefas.descricao.length > 100 && (
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
                            onPress={() => setModalVisivel(true)}
                        >
                            <Text style={styles.textoadd}>Anexar um arquivo</Text>
                        </TouchableOpacity>
                        <TouchableOpacity style={styles.botaomostrar}>
                            <Text style={styles.textoadd}>Adicionar uma mensagem</Text>
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
                                    onPress={enviarProblema}
                                >
                                    <Ionicons name="paper-plane-outline" size={24} color="#1C58F2" style={styles.iconSobreposto} /> 
                                    
                                </TouchableOpacity>
                            </View>
                        </View>
                    </View>
                </View>
            </Modal>
        </View>
    );
}