import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput } from 'react-native';
import { Dropdown } from 'react-native-element-dropdown';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';
import api from '../../../../services/api';
import fonts from "../../../styles/fonts";
export default function TarefaEnvio({ navigation, route }) {
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const equipe = route.params?.equipe || {}; 

    const [image, setImage] = useState(null);
    const [nome_equipe, setNomeEquipe] = useState();
    const [categoriaEquipe, setCategoriaEquipe] = useState('');
    const [categoriaSelecionada, setCategoriaSelecionada] = useState(null);
    const [funcionarioEquipe, setFuncionarioEquipe] = useState('');
    const [funcionarioSelecionada, setFuncionarioSelecionada] = useState(null);
    const [dados, setDados] = useState([]);
    const [dadosFuncionario, setDadosFuncionario] = useState([]);
    

    async function listarFuncionarios() {
        try {
            const res = await api.get(`dev4tec/adicionarfuncionarios.php`, {
            params: {
                id_empresa: equipe.id_empresa,
                id_equipe: equipe.id_equipe
            }
            });
            
            if (res.data.success) {
            setDadosFuncionario(res.data.result || []);
            } else {
            console.log("Erro na API:", res.data.message);
            setDadosFuncionario([]);
            }
        }
        catch (error) {
            console.log("Erro ao listar categorias", error);
        }
        }


    useEffect(() => {
        listarFuncionarios();
    }, [equipe?.id_empresa]);


    async function listarDados() {
        console.log("ID da empresa enviado:", equipe.id_empresa);
        try {
            const res = await api.get(`dev4tec/categoria.php`, {
            params: {id_empresa: equipe.id_empresa }
            });
            console.log("Resposta da API categoria:", res.data);
            
            if (res.data.success) {
            setDados(res.data.result || []);
            } else {
            console.log("Erro na API:", res.data.message);
            setDados([]);
            }
        }
        catch (error) {
            console.log("Erro ao listar categorias", error);
        }
        }


    useEffect(() => {
        listarDados();
    }, [equipe?.id_empresa]);



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

                <View style={styles.containerequipes}>
                    <View style={styles.imagem}>
                        <Image 
                        source={equipe.foto_equipe ? { uri: equipe.foto_equipe } :require('../../../../assets/img/image.png')} 
                        style={styles.imagemequipe} />
                    </View>

                    <View style={styles.textos}>
                        <Text style={styles.textoequipe}>{equipe.nome_equipe}</Text>
                        <Text style={styles.textoequipecargo}>{equipe.nome_categoria}</Text>
                    </View>
                </View>

                <View style={styles.areaInput}>
                    
                    <Text style={styles.texto}>Nome</Text>
                    <TextInput
                        style={styles.input}
                        value={nome_equipe}
                        placeholder={equipe.nome_equipe}
                        placeholderTextColor={theme.text3}
                        onChangeText={(text) => setNomeEquipe()}
                        keyboardType="numeric"
                        maxLength={10}
                    />

                    <Text style={styles.texto}>Categoria</Text>
                    <Dropdown
                        style={styles.dropdown}
                        data={dados}
                        labelField="nome_categoria" 
                        valueField="id_categoria"
                        placeholder={categoriaEquipe || "Escolha a categoria da equipe"}
                        placeholderStyle={{ color: theme.text3, fontSize: 14 }}
                        selectedTextStyle={{ color: theme.text, fontSize: 14 }}
                        value={categoriaSelecionada}
                        onChange={item => {
                            setCategoriaSelecionada(item.id_categoria);
                            setCategoriaEquipe(item.nome_categoria);
                        }}
                        containerStyle={{
                            backgroundColor: theme.inputBackground3,
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

                    <Text style={styles.texto}>Adicionar membros à equipe</Text>
                    <View style={styles.linha}>
                        <Dropdown
                            style={styles.dropdownfuncionario}
                            data={dadosFuncionario}
                            labelField="nome" 
                            valueField="FuncionarioId"
                            placeholder={funcionarioEquipe || "membros da equipe"}
                            placeholderStyle={{ color: theme.text3, fontSize: 14 }}
                            selectedTextStyle={{ color: theme.text, fontSize: 14 }}
                            value={funcionarioSelecionada}
                            onChange={item => {
                                setFuncionarioSelecionada(item.FuncionarioId);
                                setFuncionarioEquipe(item.nome);
                            }}
                            containerStyle={{
                                backgroundColor: theme.inputBackground3,
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
                        <TouchableOpacity style={styles.botaoadd}>
                            <Ionicons name="add" size={24} color="#FFFFFF" /> 
                        </TouchableOpacity>  
                    </View>

                    <TouchableOpacity 
                        style={styles.botaoeditar}
                    >
                        <Text style={styles.textoeditar}>Editar Dados</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView> 
        </View>
    );
}