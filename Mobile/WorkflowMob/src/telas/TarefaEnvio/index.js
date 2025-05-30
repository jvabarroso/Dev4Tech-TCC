import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView } from 'react-native';
import { styles } from './style';
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
        <View style={styles.container}>
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
                    <View style={styles.espacoHeader} />
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