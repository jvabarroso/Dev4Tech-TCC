import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView } from 'react-native';
import fonts from "../../styles/fonts";
import { LayoutAnimation, UIManager, Platform } from 'react-native';
import { Ionicons } from '@expo/vector-icons';

export default function TarefaEnvio({ navigation, route }) {
    const { tarefas } = route.params;
    const [descricaoExpandida, setDescricaoExpandida] = useState(false);

    if (Platform.OS === 'android' && UIManager.setLayoutAnimationEnabledExperimental) {
        UIManager.setLayoutAnimationEnabledExperimental(true);
    }

    const alternarDescricao = () => {
        LayoutAnimation.configureNext(LayoutAnimation.Presets.easeInEaseOut);
        setDescricaoExpandida(!descricaoExpandida);
    };

    return (
        <View style={styles.containerPrincipal}>
            <ScrollView 
                style={styles.scrollView}
                contentContainerStyle={styles.containerConteudo}
                showsVerticalScrollIndicator={false}
            >
                <View style={styles.nav}>
                    <TouchableOpacity 
                        style={styles.botaodevoltar}
                        onPress={() => navigation.navigate('Tarefas')}
                    >
                        <Ionicons name="arrow-back" size={24} color="black" />
                    </TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                    <View style={styles.espacoHeader} /> {/* Para centralizar o título */}
                </View>

                <View style={styles.areadetalhes}>
                    <Image source={tarefas.imagem} style={styles.imagem} />
                    <Text style={styles.titulotarefa}>{tarefas.titulo}</Text>
                    <Text style={styles.datadeenvio}>Postado em {tarefas.datadeenvio}</Text>

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
                        <TouchableOpacity style={styles.botaomostrar}>
                            <Text style={styles.textoadd}>Anexar um arquivo</Text>
                        </TouchableOpacity>
                        <TouchableOpacity style={styles.botaomostrar}>
                            <Text style={styles.textoadd}>Adicionar uma mensagem</Text>
                        </TouchableOpacity>
                        <TouchableOpacity style={styles.botaomostrar}>
                            <Text style={styles.textoproblem}>Relatar problema</Text>
                        </TouchableOpacity>
                    </View>

                    <TouchableOpacity style={styles.botaoenviar}>
                        <Text style={styles.textoenvio}>Enviar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    );
}

const styles = StyleSheet.create({
    containerPrincipal: {
        flex: 1,
        backgroundColor: "#ffffff",
    },
    scrollView: {
        flex: 1,
    },
    containerConteudo: {
        paddingHorizontal: 20,
        paddingTop: 15,
        paddingBottom: 40,
    },
    
    nav: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingVertical: 15,
        marginBottom: 10,
    },
    botaodevoltar: {
        width: 40,
        height: 40,
        justifyContent: 'center',
    },
    titulo: {
        fontSize: 18,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        textAlign: 'center',
        flex: 1,
    },
    espacoHeader: {
        width: 40,
    },
    
    areadetalhes: {
        flex: 1,
    },
    imagem: {
        width: 80,
        height: 80,
        borderRadius: 8,
        marginBottom: 15,
    },
    titulotarefa: {
        fontSize: 24,
        fontWeight: 'bold',
        fontFamily: fonts.text,
        color: "#000000",
        marginBottom: 5,
    },
    datadeenvio: {
        fontSize: 13,
        fontFamily: fonts.text,
        color: "#aaaaaa",
        marginBottom: 20,
    },
    
    linha: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginBottom: 20,
    },
    coluna: {
        flex: 1,
        marginRight: 10,
    },
    colunaEquipe: {
        flex: 1,
    },
    subtitulos: {
        fontSize: 13,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: "#181A1F",
        marginBottom: 5,
    },
    datas: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#000',
    },
    cargos: {
        backgroundColor: '#F5F7FC',
        borderRadius: 20,
        paddingHorizontal: 12,
        paddingVertical: 5,
        alignSelf: 'flex-start',
    },
    textoCargo: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#181A1F',
    },
    
    linha2: {
        flexDirection: 'column',
        marginBottom: 25,
    },
    titulodescricao: {
        fontSize: 14,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: '#000',
        marginBottom: 10,
    },
    descricao2: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#333',
        marginBottom: 5,
        lineHeight: 20,
    },
    textodescr: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#1C58F2',
        fontWeight: 'bold',
    },
    
    botaomostrar: {
        paddingVertical: 8,
    },
    textoadd: {
        color: "#1C58F2",
        fontWeight: 'bold',
        fontFamily: fonts.text,
        fontSize: 14,
    },
    textoproblem: {
        color: "#F21C1C",
        fontWeight: 'bold',
        fontFamily: fonts.text,
        fontSize: 14,
    },
    
    // Botão Enviar (original com ajustes)
    botaoenviar: {
        backgroundColor: '#1C58F2',
        paddingVertical: 12,
        paddingHorizontal: 30,
        borderRadius: 10,
        alignItems: 'center',
        alignSelf: 'center',
        marginTop: 20,
    },
    textoenvio: {
        color: '#fff',
        fontSize: 16,
        fontFamily: fonts.text,
        fontWeight: 'bold',
    },
});