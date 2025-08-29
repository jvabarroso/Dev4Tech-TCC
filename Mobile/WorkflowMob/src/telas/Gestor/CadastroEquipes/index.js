import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import { Ionicons } from '@expo/vector-icons';

export default function CadastroEquipes({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const [image, setImage] = useState(null);

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

    
    const [nome_equipe, setNome_equipe] = useState('');
    const [categoriaEquipe, setCategoriaEquipe] = useState('');

    return(
        <View style={styles.container}>
            <ScrollView contentContainerStyle={styles.scrollContent}>
                <Text style={styles.titulo}>Criar uma equipe</Text>
                <View style={styles.areafotototal}>
                    <View style={styles.areafoto}>

                    <TouchableOpacity 
                        style={styles.buttonEnviar} 
                    >
                        <Ionicons name="cloud-upload" size={20} color="white"/>
                        <Text style={styles.buttonText}>Enviar Imagem</Text>
                    </TouchableOpacity>

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
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite o nome da equipe"
                        value={nome_equipe}
                        onChangeText={setNome_equipe} 
                        placeholderTextColor={theme.text}
                    />
                    <Text style={styles.texto}>Categoria da equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite a categoria da equipe"
                        value={categoriaEquipe}
                        onChangeText={setCategoriaEquipe} 
                        placeholderTextColor={theme.text}
                        secureTextEntry={true}
                    />

                    <TouchableOpacity 
                        style={styles.botaocriar}
                    >
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
