import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput, Modal } from 'react-native';
import { Dropdown } from 'react-native-element-dropdown';
import FlashMessage, { showMessage } from "react-native-flash-message";
import * as ImagePicker from "expo-image-picker";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import url from '../../../../services/url';
import api from '../../../../services/api';
import fonts from "../../../styles/fonts";

export default function EditEquipe({ navigation, route }) {
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const equipe = route.params?.equipe || {}; 

    const [nome_equipe, setNomeEquipe] = useState();
    const [categoriaEquipe, setCategoriaEquipe] = useState('');
    const [categoriaSelecionada, setCategoriaSelecionada] = useState(null);
    const [modalVisivel, setModalVisivel] = useState(false);

    const [funcionarioEquipe, setFuncionarioEquipe] = useState('');
    const [funcionarioSelecionada, setFuncionarioSelecionada] = useState(null);

    const [dados, setDados] = useState([]);
    const [dadosFuncionario, setDadosFuncionario] = useState([]);

    const [funcionariosEquipeArray, setFuncionariosEquipeArray] = useState([]); 

    const [image, setImage] = useState(null);
    const [imagemEquipe, setImagemEquipe] = useState(null);

    //Mostra os dados atuais da equipe
    useEffect(() => {
        if (equipe?.foto_equipe) {
            setImage(equipe.foto_equipe);
        }
        setNomeEquipe(equipe.nome_equipe || '');
        setCategoriaSelecionada(equipe.id_categoria || '');
        setCategoriaEquipe(equipe.nome_categoria || '');
    }, [equipe]);

    //Adicionar funcionario na equipe
    function adicionarFuncionario() {
        if (funcionarioSelecionada) {
            // Evita adicionar duplicados
            const jaExiste = funcionariosEquipeArray.some(
                f => f.FuncionarioId === funcionarioSelecionada
            );

            if (!jaExiste) {
                setFuncionariosEquipeArray(prev => [
                    ...prev, 
                    { FuncionarioId: funcionarioSelecionada, nome: funcionarioEquipe }
                ]);
            }
        }
    }
    //Lista apenas os funcionarios que não estão na equipe
    async function listarFuncionarios() {
        try {
            const res = await api.get(`dev4tech/funcionariosadm.php`, {
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


    //Lista as categorias
    async function listarDados() {
        console.log("ID da empresa enviado:", equipe.id_empresa);
        try {
            const res = await api.get(`dev4tech/categoria.php`, {
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

    //Imagem da galeria
    async function pickImageFromGallery() {
        let result = await ImagePicker.launchImageLibraryAsync({
            mediaTypes: ImagePicker.MediaTypeOptions.Images,
            allowsEditing: true,
            aspect: [4, 3],
            quality: 1,
        });

        if (!result.canceled) {
            console.log(result); // Verificar o retorno completo
            setImage(result.assets[0].uri); // Acesse o URI corretamente
        }
        }

        async function takePhoto() {
        let result = await ImagePicker.launchCameraAsync({
            allowsEditing: true,
            aspect: [4, 3],
            quality: 1,
        });

        if (!result.canceled) {
            console.log(result); // Verificar o retorno completo
            setImage(result.assets[0].uri); // Acesse o URI corretamente
        }
        }

    //Tirar foto
    async function uploadImage() {
        if (!image) {
            showMessage({
            message: 'Nenhuma imagem selecionada.',
            description: 'Por favor, selecione ou tire uma foto primeiro.',
            floating: true,
            statusBarHeight: 70,
            type: "danger",
            duration: 2000,             
            });
            return false;
        }

        let filename = image.split('/').pop();
        let match = /\.(\w+)$/.exec(filename);
        let type = match ? `image/${match[1]}` : `image`;

        let formData = new FormData();
        formData.append('photo', { uri: image, name: filename, type });

        try {
            const response = await fetch(`${url}/dev4tech/upload_equipe.php`, {
                method: 'POST',
                body: formData,
            });
            
            const resJson = await response.json();
                        
            if (resJson.success) {
                showMessage({
                    message: 'Sucesso.',
                    description: 'Imagem enviada com sucesso!',
                    floating: true,
                    statusBarHeight: 70,
                    type: "success",
                    duration: 2000,             
                });
                // devolve a URL (se o PHP retornar url) ou o nome do arquivo
                return resJson.url ?? resJson.file ?? resJson;
                } else {
                showMessage({
                    message: 'Erro.',
                    description: resJson.message || 'Falha ao enviar imagem.',
                    type: "warning",
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

    //Post para o Banco:
    async function editar() {  
        let foto_equipe = null;
        if (image) {
            const uploadResult = await uploadImage();
            if (!uploadResult) return; 
            foto_equipe = uploadResult; 
        } else {
            // manter foto atual (tente pegar equipe.foto_equipe ou equipe.imagem)
            foto_equipe = equipe.foto_equipe ?? equipe.imagem ?? null;
        }
        if (!nome_equipe|| !categoriaSelecionada) {
            showMessage({
                message: 'Erro.',
                description: 'Preencha todos os campos obrigatórios!',
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 2000,             
            });
         return;
     }
     try {
        const obj = {
          id: equipe.id_equipe,
          nome_equipe: nome_equipe,
          id_categoria: categoriaSelecionada,
          foto_equipe: foto_equipe,
          funcionarios: funcionariosEquipeArray
        };

         console.log('Dados enviados para edição:', obj); // Log para debug

          const res = await api.post('dev4tech/editarequipe.php', obj, {
            headers: {
              'Content-Type': 'application/json',
            }
          });

         console.log('Resposta da API:', res.data); // Log para 
         
        if (res.data.success) {
            showMessage({
                message: 'Sucesso.',
                description: 'Dados atualizados com sucesso!',
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
            });

            setImagemEquipe(prev => ({
            ...prev,
            [equipe.id_equipe]: foto_equipe
            }));
        } else {
            showMessage({
                message: 'Erro.',
                description: res.data.message || "Erro ao atualizar dados",
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 2000,             
            });
        }
    } catch (error) {
        console.error("Erro completo:", error);
            showMessage({
                message: 'Erro.',
                description: "Não foi possível conectar ao servidor",
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
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
                        <Ionicons name="arrow-back" size={24} color={theme.text} />
                    </TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                    <View style={styles.espacoHeader} />
                </View>

                <View style={styles.containerequipes}>
                    <View style={styles.imagem}>
                        <TouchableOpacity onPress={() => setModalVisivel(true)}>
                            <Image 
                                source={image ? { uri: image } :require('../../../../assets/img/image.png')} 
                                style={styles.imagemequipe} />
                        </TouchableOpacity>
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
                        onChangeText={(text) => setNomeEquipe(text)}
                        keyboardType="default"
                        maxLength={50}
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
                        <TouchableOpacity 
                            style={styles.botaoadd}
                            onPress={adicionarFuncionario}
                        >
                            <Ionicons name="add" size={24} color="#FFFFFF" /> 
                        </TouchableOpacity>  
                    </View>

                    <TouchableOpacity 
                        style={styles.botaoeditar}
                        onPress={editar}
                    >
                        <Text style={styles.textoeditar}>Editar Dados</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView> 
            <Modal
            animationType="slide"
            transparent={true}
            visible={modalVisivel}
            onRequestClose={() => setModalVisivel(false)}
            >   
                <View style={styles.modalContainer}>
                    <View style={styles.modalContent}>
                    <View style={styles.nav2}>
                        <TouchableOpacity 
                        style={styles.botaodevoltar}
                        onPress={() => setModalVisivel(false)}
                        >
                        <Ionicons name="close-outline" size={36} color={theme.text} />
                        </TouchableOpacity>
                    </View>
                    <View style={styles.areafotototal}>
                        <View style={styles.areatitulofoto}>
                        <Text style={styles.textfoto}>Selecione uma foto</Text>
                        </View>
                        <View style={styles.areafoto}>

                        <Image 
                            source={image ? { uri: image } :require('../../../../assets/img/image.png')} 
                            style={styles.imagemPreview} />

                        <View style={styles.areafoto2}>
                            <TouchableOpacity 
                            style={styles.button} 
                            onPress={pickImageFromGallery}
                            >
                            <Text style={styles.buttonText2}>Escolher da Galeria</Text>
                            </TouchableOpacity>
                    
                            <TouchableOpacity 
                            style={styles.button} 
                            onPress={takePhoto}
                            >   
                            <Text style={styles.buttonText2}>Tirar Foto</Text>
                            </TouchableOpacity>   
                        </View>
                        </View>
                    </View>
                    </View>
                    <FlashMessage position="top" />
                </View>
            </Modal>
        </View>
    );
}